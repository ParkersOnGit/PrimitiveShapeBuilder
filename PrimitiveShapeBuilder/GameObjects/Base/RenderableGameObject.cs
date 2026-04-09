using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using PrimitiveShapeBuilder.Loaders;

namespace PrimitiveShapeBuilder.GameObjects.Base
{
    internal abstract class RenderableGameObject : GameObject
    {
        private int vertexBufferObject;
        private int elementBufferObject;
        private int vertexArrayObject;

        internal Shader shader;
        private Model model;

        protected string vertexShaderPath;
        protected string fragmentShaderPath;
        protected string modelPath;
        protected Matrix4 modelMatrix;

        

        internal void Initialize()
        {
            // initialize the model
            model = new Model();
            model.LoadModel(modelPath);


            // generate the vao
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);


            // generate the vbo
            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, model.Data.Length * sizeof(float), model.Data, BufferUsageHint.StaticDraw);

            // upload vertex data
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // upload normal data
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // upload texture data
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);


            // generate the ebo
            elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, model.Indices.Length * sizeof(uint), model.Indices, BufferUsageHint.StaticDraw);


            // use the shader
            shader = new Shader(vertexShaderPath, fragmentShaderPath);
            shader.Use();
        }

        internal void Render(Matrix4 viewMatrix, Matrix4 projectionMatrix)
        {
            Update();

            GL.BindVertexArray(vertexArrayObject);

            shader.SetMatrix4("model", modelMatrix);
            shader.SetMatrix4("view", viewMatrix);
            shader.SetMatrix4("projection", projectionMatrix);
            shader.Use();

            GL.DrawElements(PrimitiveType.Triangles, model.Indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        private void Update()
        {
            // reset the model matrix
            modelMatrix = Matrix4.Identity;

            // scale
            modelMatrix *= Matrix4.CreateScale(Scale.X, Scale.Y, Scale.Z);

            // rotate (yaw, pitch, then roll)
            modelMatrix *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotation.Y)) *
                Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotation.X)) *
                Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation.Z));

            // transpose
            modelMatrix *= Matrix4.CreateTranslation(Position);
        }
    }
}

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

        private float[] vertices;
        private uint[] indices;

        public Shader shader;

        protected string vertexShaderPath;
        protected string fragmentShaderPath;
        protected string modelPath;
        protected Matrix4 model;

        

        public void Initialize()
        {
            // generate the vao
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            // generate the vbo
            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

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
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            // use the shader
            shader = new Shader(vertexShaderPath, fragmentShaderPath);
            shader.Use();
        }

        public void Render()
        {
            Update();

            GL.BindVertexArray(vertexArrayObject);

            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", Window.Camera.View);
            shader.SetMatrix4("projection", Window.Projection);
            shader.Use();

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        private void Update()
        {
            // reset the model matrix
            model = Matrix4.Identity;

            // scale
            model *= Matrix4.CreateScale(Scale.X, Scale.Y, Scale.Z);

            // rotate (yaw, pitch, then roll)
            model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotation.Y)) *
                Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotation.X)) *
                Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation.Z));

            // transpose
            model *= Matrix4.CreateTranslation(Position);
        }
    }
}

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

        internal string vertexShaderPath;
        internal string fragmentShaderPath;
        internal string modelPath;
        protected Matrix4 modelMatrix;

        internal Vector3 Color;
        
        internal void Initialize()
        {
            model = new Model();
            model.LoadModel(modelPath);

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, model.Data.Length * sizeof(float), model.Data, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, model.Indices.Length * sizeof(uint), model.Indices, BufferUsageHint.StaticDraw);

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
            modelMatrix = Matrix4.Identity;

            modelMatrix *= Matrix4.CreateScale(Scale.X, Scale.Y, Scale.Z);

            modelMatrix *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotation.Y)) *
                Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotation.X)) *
                Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation.Z));

            modelMatrix *= Matrix4.CreateTranslation(Position);
        }
    }
}

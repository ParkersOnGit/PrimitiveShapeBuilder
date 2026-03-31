using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace PrimitiveShapeBuilder.GameObjects.Base
{
    internal abstract class RenderableGameObject : GameObject
    {
        private int vertexBufferObject;
        private int normalBufferObject;
        private int elementBufferObject;
        private int vertexArrayObject;


        private float[] vertices;
        private float[] normals;
        private uint[] indices;

        public Shader shader;

        protected string vertexShaderPath;
        protected string fragmentShaderPath;
        protected string modelPath;
        protected Matrix4 model;

        public void LoadModel()
        {
            // create the vertices and indices arrays
            List<float> tempVertices = new List<float>();
            List<float> tempNormals = new List<float>();
            List<uint> tempIndices = new List<uint>();

            // loop through all the lines in the source obj files
            foreach (string line in File.ReadAllLines(modelPath))
            {
                if (line.StartsWith("v "))
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        tempVertices.Add(float.Parse(line.Split(' ')[i]));
                    }
                }
                else if (line.StartsWith("vn "))
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        tempNormals.Add(float.Parse(line.Split(' ')[i]));
                    }
                }
                else if (line.StartsWith("f "))
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        tempIndices.Add(uint.Parse(line.Split(' ')[i].Split('/')[0]) - 1);
                    }
                }
            }

            // convert list to arrays
            vertices = tempVertices.ToArray();
            normals = tempNormals.ToArray();
            indices = tempIndices.ToArray();

            //// PROBABLY MAKE  THIS CODE BETTER SOMWHOW BECAUSE IT PROBABLY IS NOT THAT EFFECIENT




        }

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
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // generate the nbo
            normalBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, normalBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, normals.Length * sizeof(float), normals, BufferUsageHint.StaticDraw);
            // upload normal data
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);

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
            shader.SetMatrix4("view", Window.camera.View);
            shader.SetMatrix4("projection", Window.projection);
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

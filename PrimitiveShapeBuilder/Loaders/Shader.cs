using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace PrimitiveShapeBuilder.Loaders
{
    internal class Shader
    {
        private int Handle;
        private Dictionary<string, int> uniformLocations = new Dictionary<string, int>();

        public Shader(string vertexPath, string fragmentPath)
        {
            string vertexShaderSource = File.ReadAllText(vertexPath);
            string fragmentShaderSource = File.ReadAllText(fragmentPath);

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            int fragementShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragementShader, fragmentShaderSource);

            Compile(vertexShader);
            Compile(fragementShader);

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragementShader);

            Link();

            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragementShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragementShader);

            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int count);
            for (int i = 0; i < count; i++)
            {
                string key = GL.GetActiveUniform(Handle, i, out _, out _);
                int location = GL.GetUniformLocation(Handle, key);
                uniformLocations.Add(key, location);
            }

        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        private void Compile(int shader)
        {
            GL.CompileShader(shader);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string log = GL.GetShaderInfoLog(shader);
                Console.WriteLine(log);
            }
        }

        private void Link()
        {
            GL.LinkProgram(Handle);
            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string log = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(log);
            }
        }

        public void SetInt(string name, int data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(uniformLocations[name], data);
        }
        public void SetFloat(string name, float data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(uniformLocations[name], data);
        }
        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(uniformLocations[name], true, ref data);
        }
        public void SetVector3(string name, Vector3 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform3(uniformLocations[name], data);
        }
    }
}

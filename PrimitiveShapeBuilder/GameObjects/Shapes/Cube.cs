using PrimitiveShapeBuilder.GameObjects.Base;

namespace PrimitiveShapeBuilder.GameObjects.Shapes
{
    internal class Cube : RenderableGameObject
    {
        internal Cube()
        {
            modelPath = "../../../Assets/Models/Cube.obj";
            vertexShaderPath = "../../../Assets/Shaders/FlatStyle.vert";
            fragmentShaderPath = "../../../Assets/Shaders/FlatStyle.frag";
        }
    }
}

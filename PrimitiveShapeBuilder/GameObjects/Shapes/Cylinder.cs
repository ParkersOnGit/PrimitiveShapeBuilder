using PrimitiveShapeBuilder.GameObjects.Base;

namespace PrimitiveShapeBuilder.GameObjects.Shapes
{
    internal class Cylinder : RenderableGameObject
    {
        internal Cylinder()
        {
            modelPath = "../../../Assets/Models/Cylinder.obj";
            vertexShaderPath = "../../../Assets/Shaders/Default.vert";
            fragmentShaderPath = "../../../Assets/Shaders/Lighting.frag";
        }
    }
}

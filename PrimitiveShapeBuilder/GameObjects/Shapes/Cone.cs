using PrimitiveShapeBuilder.GameObjects.Base;

namespace PrimitiveShapeBuilder.GameObjects.Shapes
{
    internal class Cone : RenderableGameObject
    {
        internal Cone()
        {
            modelPath = "../../../Assets/Models/Cone.obj";
            vertexShaderPath = "../../../Assets/Shaders/Default.vert";
            fragmentShaderPath = "../../../Assets/Shaders/Lighting.frag";
        }
    }
}

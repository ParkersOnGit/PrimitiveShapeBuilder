using PrimitiveShapeBuilder.GameObjects.Base;

namespace PrimitiveShapeBuilder.GameObjects.Shapes
{
    internal class TriangularPrism : RenderableGameObject
    {
        internal TriangularPrism()
        {
            modelPath = "../../../Assets/Models/TriangularPrism.obj";
            vertexShaderPath = "../../../Assets/Shaders/Default.vert";
            fragmentShaderPath = "../../../Assets/Shaders/Lighting.frag";
        }
    }
}

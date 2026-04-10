using PrimitiveShapeBuilder.GameObjects.Base;

namespace PrimitiveShapeBuilder.GameObjects.Shapes
{
    internal class SquarePyramid : RenderableGameObject
    {
        internal SquarePyramid()
        {
            modelPath = "../../../Assets/Models/SquarePyramid.obj";
            vertexShaderPath = "../../../Assets/Shaders/Default.vert";
            fragmentShaderPath = "../../../Assets/Shaders/Lighting.frag";
        }
    }
}

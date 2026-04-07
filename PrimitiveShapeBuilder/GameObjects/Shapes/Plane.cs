using PrimitiveShapeBuilder.GameObjects.Base;

namespace PrimitiveShapeBuilder.GameObjects.Shapes
{
    internal class Plane : RenderableGameObject
    {
        internal Plane()
        {
            modelPath = "../../../Assets/Models/Plane.obj";
            vertexShaderPath = "../../../Assets/Shaders/Grid.vert";
            fragmentShaderPath = "../../../Assets/Shaders/Grid.frag";
        }
    }
}

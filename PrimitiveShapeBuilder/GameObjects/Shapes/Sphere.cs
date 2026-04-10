using PrimitiveShapeBuilder.GameObjects.Base;

namespace PrimitiveShapeBuilder.GameObjects.Shapes
{
    internal class Sphere : RenderableGameObject
    {
        internal Sphere()
        {
            modelPath = "../../../Assets/Models/Sphere.obj";
            vertexShaderPath = "../../../Assets/Shaders/Default.vert";
            fragmentShaderPath = "../../../Assets/Shaders/Lighting.frag";
        }
    }
}

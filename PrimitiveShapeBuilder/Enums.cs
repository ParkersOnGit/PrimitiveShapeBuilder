using OpenTK.Mathematics;
using PrimitiveShapeBuilder.GameObjects.Base;
using PrimitiveShapeBuilder.GameObjects.Shapes;

namespace PrimitiveShapeBuilder
{
    internal static class Enums
    {
        internal enum ShapeType
        {
            Cube,
            SquarePyramid,
            Plane,
        }

        internal static RenderableGameObject ToShapeObject(this ShapeType shape)
        {
            switch (shape)
            {
                case ShapeType.Cube: return new Cube();
                //case ShapeType.SquarePyramid: return new SquarePyramid();
                case ShapeType.Plane: return new Plane();
                default: return null;
            }
        }

        internal enum ColorType
        {
            Red,
            Orange,
            Yellow,
            Green,
            Blue,
            Purple,
            White,
            Gray,
            Black
        }

        internal static Vector3 ToColor(this ColorType color)
        {
            switch (color)
            {
                case ColorType.Red: return new Vector3(1.0f, 0.0f, 0.0f);
                case ColorType.Orange: return new Vector3(0.0f, 0.0f, 0.0f); // TODO: Fix this color
                case ColorType.Yellow: return new Vector3(1.0f, 0.0f, 0.0f); // TODO: Fix this color
                case ColorType.Green: return new Vector3(0.0f, 1.0f, 0.0f);
                case ColorType.Blue: return new Vector3(0.0f, 0.0f, 1.0f); 
                case ColorType.Purple: return new Vector3(1.0f, 0.0f, 0.0f); // TODO: Fix this color
                case ColorType.White: return new Vector3(1.0f, 1.0f, 1.0f); 
                case ColorType.Gray: return new Vector3(0.5f, 0.5f, 0.5f); 
                case ColorType.Black: return new Vector3(0.0f, 0.0f, 0.0f); 
                default: return new Vector3(1.0f); 
            }
        }
    }
}

using OpenTK.Mathematics;

namespace PrimitiveShapeBuilder.GameObjects.Base
{
    internal abstract class GameObject
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        private Vector3 scale;
        public Vector3 Scale
        {
            get => scale;
            set
            {
                value = (MathF.Max(0, value.X), MathF.Max(0, value.Y), MathF.Max(0, value.Z));
                scale = value;
            }
        }

        protected GameObject()
        {
            Position = (0, 0, 0);
            Rotation = (0, 0, 0);
            Scale = (1, 1, 1);
        }
    }
}

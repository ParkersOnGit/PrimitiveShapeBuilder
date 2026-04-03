using OpenTK.Mathematics;
using PrimitiveShapeBuilder.GameObjects.Base;

namespace PrimitiveShapeBuilder.GameObjects
{
    internal class Camera : GameObject
    {
        private Vector3 rotation;
        public Vector3 Rotation
        {
            get => rotation;
            set
            {
                value.X = MathF.Min(MathF.Max(-90.0f, value.X), 90.0f);
                rotation = value;
            }
        }

        public Matrix4 View { get; private set; }

        public Camera(Vector3? position = null)
        {
            Position = position ?? (0.0f, 0.0f, 0.0f);
        }

        public void Update()
        {
            // reset the view matrix
            View = Matrix4.Identity;

            // transpose
            View *= Matrix4.CreateTranslation(Position);

            // rotate (yaw, pitch, then roll)
            View *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotation.Y)) *
                Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotation.X)) *
                Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation.Z));
        }
    }
}

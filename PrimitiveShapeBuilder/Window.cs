using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using PrimitiveShapeBuilder.GameObjects;
using PrimitiveShapeBuilder.GameObjects.Shapes;

namespace PrimitiveShapeBuilder
{
    internal class Window : GameWindow
    {
        internal Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) { }

        internal static Matrix4 Projection { get; private set; }
        internal static Camera Camera = new Camera();

        private Plane gridPlane = new Plane(); // grid plane (do not remove)

        // new cube object (remove later)
        private Cube cube = new Cube();

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.0f, 0.0f, 0.2f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            UpdateView();

            gridPlane.Initialize();

            CursorState = CursorState.Grabbed;// make option later

            cube.Initialize();


            // make the window visible after loading everything
            IsVisible = true;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Camera.Update();

            gridPlane.Position = new Vector3(Camera.Position.X, 0.0f, Camera.Position.Z);
            gridPlane.Render();

            cube.Render();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            double DT = e.Time; // delta time
            KeyboardState KB = KeyboardState;
            MouseState MS = MouseState;



            // close the program (remove later maybe)
            if (KB.IsKeyDown(Keys.Escape))
                Close();







            // camera position
            float xVector = (float)MathHelper.Sin(MathHelper.DegreesToRadians(Camera.Rotation.Y)) * 5;
            float zVector = (float)MathHelper.Cos(MathHelper.DegreesToRadians(Camera.Rotation.Y)) * 5;

            if (KB.IsKeyDown(Keys.W))
                Camera.Position += new Vector3(-xVector, 0.0f, -zVector) * (float)DT;
            if (KB.IsKeyDown(Keys.A))
                Camera.Position += new Vector3(-zVector, 0.0f, xVector) * (float)DT;
            if (KB.IsKeyDown(Keys.S))
                Camera.Position += new Vector3(xVector, 0.0f, zVector) * (float)DT;
            if (KB.IsKeyDown(Keys.D))
                Camera.Position += new Vector3(zVector, 0.0f, -xVector) * (float)DT;
            if (KB.IsKeyDown(Keys.Space))
                Camera.Position += new Vector3(0.0f, 5.0f, 0.0f) * (float)DT;
            if (KB.IsKeyDown(Keys.LeftShift))
                Camera.Position += new Vector3(0.0f, -5.0f, 0.0f) * (float)DT;

            // camera rotation
            Camera.Rotation += new Vector3(-MS.Delta.Y, -MS.Delta.X, 0.0f) * 0.1f;
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            UpdateView();
        }

        private void UpdateView()
        {
            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90), ClientSize.X / (float)ClientSize.Y, 0.001f, 100.0f);
        }
    }
}

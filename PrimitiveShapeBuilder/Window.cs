using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using PrimitiveShapeBuilder.GameObjects;
using PrimitiveShapeBuilder.GameObjects.Base;
using PrimitiveShapeBuilder.GameObjects.Shapes;

namespace PrimitiveShapeBuilder
{
    internal class Window : GameWindow
    {
        internal Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) { }

        internal static Matrix4 Projection { get; private set; }
        internal static Camera Camera = new Camera((0, 2, 0));
        private static Camera UICamera = new Camera((0, 0, 3));

        private bool F1Pressed, F11Pressed, TabPressed, RightClickPressed = false; // key switch bools
        private bool ShowUI = true;

        private Plane gridPlane = new Plane(); // grid plane (do not remove)
        private RenderableGameObject UIObject = new Cube();

        private List<RenderableGameObject> shapes = new List<RenderableGameObject>();

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            UpdateView();

            gridPlane.Initialize();
            gridPlane.Scale = new Vector3(10.0f, 0.0f, 10.0f);

            UIObject.Initialize();
            UIObject.Rotation = new Vector3(0.0f, 45.0f, 0.0f);
            UIObject.Scale = new Vector3(0.2f);
            UIObject.Position = new Vector3(-2.0f, 0.0f, 0.0f);

            UICamera.Update();


            CursorState = CursorState.Grabbed;
            // make the window visible after loading everything
            IsVisible = true;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Camera.Update();


            RenderAllObjects(shapes);

            if (ShowUI)
            {
                //UIObject.Render(UICamera.View, Projection);

                gridPlane.shader.SetVector3("cameraPosition", Camera.Position);
                gridPlane.Position = new Vector3(Camera.Position.X, 0.0f, Camera.Position.Z);
                gridPlane.Render(Camera.View, Projection);
            }

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            double DT = e.Time; // delta time
            KeyboardState KB = KeyboardState;
            MouseState MS = MouseState;


            // close the program
            if (KB.IsKeyDown(Keys.Escape))
                Close();

            // full screen toggle
            if (KB.IsKeyDown(Keys.F11) && !F11Pressed)
            {
                WindowState = WindowState == WindowState.Fullscreen ? WindowState.Normal : WindowState.Fullscreen;
                F11Pressed = true;
            }
            else if (KB.IsKeyReleased(Keys.F11) && F11Pressed) F11Pressed = false;

            // toggle cursor state
            if (KB.IsKeyDown(Keys.Tab) && !TabPressed)
            {
                CursorState = CursorState == CursorState.Grabbed ? CursorState.Normal : CursorState.Grabbed;
                TabPressed = true;
            }
            else if (KB.IsKeyReleased(Keys.Tab) && TabPressed) TabPressed = false;

            // toggle UI
            if (KB.IsKeyDown(Keys.F1) && !F1Pressed)
            {
                ShowUI = !ShowUI;
                F1Pressed = true;
            }
            else if (KB.IsKeyReleased(Keys.F1) && F1Pressed) F1Pressed = false;


            // create object view object placement
            if (MS.IsButtonDown(MouseButton.Right) && !RightClickPressed)
            {
                Cube newCube = new Cube();
                Vector3 forwardVector = new Vector3(
                    (float)(Math.Cos(MathHelper.DegreesToRadians(Camera.Rotation.X)) * -Math.Sin(MathHelper.DegreesToRadians(Camera.Rotation.Y))),
                    (float)Math.Sin(MathHelper.DegreesToRadians(Camera.Rotation.X)),
                    (float)(Math.Cos(MathHelper.DegreesToRadians(Camera.Rotation.X)) * -Math.Cos(MathHelper.DegreesToRadians(Camera.Rotation.Y)))
                );
                newCube.Position = Camera.Position + forwardVector * 5;
                newCube.Initialize();
                shapes.Add(newCube);
                RightClickPressed = true;
            }
            else if (MS.IsButtonReleased(MouseButton.Right) && RightClickPressed) RightClickPressed = false;





            // camera position
            float speed = 5.0f;

            if (KB.IsKeyDown(Keys.W))
                Camera.Position += new Vector3(-(float)MathHelper.Sin(MathHelper.DegreesToRadians(Camera.Rotation.Y)), 0.0f, -(float)MathHelper.Cos(MathHelper.DegreesToRadians(Camera.Rotation.Y))) * (float)DT * speed;
            if (KB.IsKeyDown(Keys.A))
                Camera.Position += new Vector3(-(float)MathHelper.Cos(MathHelper.DegreesToRadians(Camera.Rotation.Y)), 0.0f, (float)MathHelper.Sin(MathHelper.DegreesToRadians(Camera.Rotation.Y))) * (float)DT * speed;
            if (KB.IsKeyDown(Keys.S))
                Camera.Position += new Vector3((float)MathHelper.Sin(MathHelper.DegreesToRadians(Camera.Rotation.Y)), 0.0f, (float)MathHelper.Cos(MathHelper.DegreesToRadians(Camera.Rotation.Y))) * (float)DT * speed;
            if (KB.IsKeyDown(Keys.D))
                Camera.Position += new Vector3((float)MathHelper.Cos(MathHelper.DegreesToRadians(Camera.Rotation.Y)), 0.0f, -(float)MathHelper.Sin(MathHelper.DegreesToRadians(Camera.Rotation.Y))) * (float)DT * speed;
            if (KB.IsKeyDown(Keys.Space))
                Camera.Position += new Vector3(0.0f, 1.0f, 0.0f) * (float)DT * speed;
            if (KB.IsKeyDown(Keys.LeftShift))
                Camera.Position += new Vector3(0.0f, -1.0f, 0.0f) * (float)DT * speed;

            // camera rotation
            if (CursorState == CursorState.Grabbed)
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

        // needed because apcsp requires a method with specific stuff
        private void RenderAllObjects(List<RenderableGameObject> shapesList)
        {
            foreach (RenderableGameObject shape in shapesList)
            {
                if (shape == null) continue;
                shape.shader.SetVector3("lightPos", Camera.Position);
                shape.shader.SetVector3("objectColor", new Vector3(0.5f, 0.5f, 1.0f));
                shape.Render(Camera.View, Projection);
            }
        }
    }
}

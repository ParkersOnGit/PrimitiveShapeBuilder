using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using PrimitiveShapeBuilder.GameObjects;
using PrimitiveShapeBuilder.GameObjects.Base;
using PrimitiveShapeBuilder.GameObjects.Shapes;
using static PrimitiveShapeBuilder.Enums;

namespace PrimitiveShapeBuilder
{
    internal class Window : GameWindow
    {
        internal Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) { }

        internal static Matrix4 Projection { get; private set; }
        internal static Camera Camera = new Camera((0, 2, 0));

        private bool F1Pressed, F11Pressed, TabPressed, RightClickPressed = false;
        private bool ShowUI = true;

        private ShapeType currentShapeType = ShapeType.Cube;
        private ColorType currentColorType = ColorType.White;

        private Plane gridPlane = new Plane();
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

            UIObject = currentShapeType.ToShapeObject();
            UIObject.Rotation = new Vector3(25.0f, 45.0f, 180.0f);
            UIObject.Position = new Vector3(100f, ClientSize.Y - 100f, 0f);
            UIObject.Scale = new Vector3(50.0f);
            UIObject.fragmentShaderPath = "../../../Assets/Shaders/FlatStyle.frag";
            UIObject.Initialize();

            CursorState = CursorState.Grabbed;
            IsVisible = true;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Camera.Update();

            RenderAllShapes(shapes);

            if (ShowUI)
            {
                gridPlane.shader.SetVector3("cameraPosition", Camera.Position);
                gridPlane.Position = new Vector3(Camera.Position.X, 0.0f, Camera.Position.Z);
                gridPlane.Render(Camera.View, Projection);

                UIObject.Rotation += new Vector3(0, 25 * (float)e.Time, 0);
                Matrix4 uiProjection = Matrix4.CreateOrthographicOffCenter(0, ClientSize.X, ClientSize.Y, 0, -100f, 100f);
                GL.Clear(ClearBufferMask.DepthBufferBit);
                UIObject.shader.SetVector3("objectColor", currentColorType.ToColor());
                UIObject.Render(Matrix4.Identity, uiProjection);
            }

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            double DT = e.Time;
            KeyboardState KB = KeyboardState;
            MouseState MS = MouseState;

            if (KB.IsKeyDown(Keys.Escape))
                Close();

            if (KB.IsKeyDown(Keys.F11) && !F11Pressed)
            {
                WindowState = WindowState == WindowState.Fullscreen ? WindowState.Normal : WindowState.Fullscreen;
                F11Pressed = true;
            }
            else if (KB.IsKeyReleased(Keys.F11) && F11Pressed) F11Pressed = false;

            if (KB.IsKeyDown(Keys.Tab) && !TabPressed)
            {
                CursorState = CursorState == CursorState.Grabbed ? CursorState.Normal : CursorState.Grabbed;
                TabPressed = true;
            }
            else if (KB.IsKeyReleased(Keys.Tab) && TabPressed) TabPressed = false;

            if (KB.IsKeyDown(Keys.F1) && !F1Pressed)
            {
                ShowUI = !ShowUI;
                F1Pressed = true;
            }
            else if (KB.IsKeyReleased(Keys.F1) && F1Pressed) F1Pressed = false;


            if (MS.IsButtonDown(MouseButton.Right) && !RightClickPressed)
            {
                RightClickPressed = true;
            }
            else if (MS.IsButtonReleased(MouseButton.Right) && RightClickPressed)
            {
                CreateShape(currentShapeType, currentColorType);
                RightClickPressed = false;
            }

            if (KB.IsKeyDown(Keys.LeftControl) || KB.IsKeyDown(Keys.RightControl))
            {
                if (MS.ScrollDelta.Y < 0)
                    currentColorType = currentColorType.Increment();
                else if (MS.ScrollDelta.Y > 0)
                    currentColorType = currentColorType.Decrement();
            }
            else
            {
                if (MS.ScrollDelta.Y < 0)
                    currentShapeType = currentShapeType.Increment();
                else if (MS.ScrollDelta.Y > 0)
                    currentShapeType = currentShapeType.Decrement();
            }
            if (MS.ScrollDelta.Y != 0 && !(KB.IsKeyDown(Keys.LeftControl) || KB.IsKeyDown(Keys.RightControl)))
            {
                UIObject = currentShapeType.ToShapeObject();
                UIObject.Rotation = new Vector3(25.0f, 45.0f, 180.0f);
                UIObject.Position = new Vector3(100f, ClientSize.Y - 100f, 0f);
                UIObject.Scale = new Vector3(50.0f);
                UIObject.fragmentShaderPath = "../../../Assets/Shaders/FlatStyle.frag";
                UIObject.Initialize();
            }

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
            UIObject.Position = new Vector3(100f, ClientSize.Y - 100f, 0f);
        }

        private void CreateShape(ShapeType shapeType, ColorType colorType)
        {
            RenderableGameObject newShape = shapeType.ToShapeObject();
            Vector3 forwardVector = new Vector3(
                (float)(Math.Cos(MathHelper.DegreesToRadians(Camera.Rotation.X)) * -Math.Sin(MathHelper.DegreesToRadians(Camera.Rotation.Y))),
                (float)Math.Sin(MathHelper.DegreesToRadians(Camera.Rotation.X)),
                (float)(Math.Cos(MathHelper.DegreesToRadians(Camera.Rotation.X)) * -Math.Cos(MathHelper.DegreesToRadians(Camera.Rotation.Y)))
            );
            
            newShape.Position = Camera.Position + forwardVector * 5;
            newShape.Color = colorType.ToColor();

            newShape.Initialize();
            shapes.Add(newShape);
        }

        private void RenderAllShapes(List<RenderableGameObject> shapesList)
        {
            foreach (RenderableGameObject shape in shapesList)
            {
                if (shape == null) continue;
                shape.shader.SetVector3("lightPos", Camera.Position);
                shape.shader.SetVector3("objectColor", shape.Color);
                shape.Render(Camera.View, Projection);
            }
        }

        private void SpawnAllObjectsColors()
        {
            for (int y = 0; y <= (int)Enum.GetValues(typeof(ShapeType)).Cast<ShapeType>().Max(); y++)
            {
                for (int x = 0; x <= (int)Enum.GetValues(typeof(ColorType)).Cast<ColorType>().Max(); x++)
                {
                    Camera.Position = new Vector3(x * 3, 2, y * 3);
                    CreateShape((ShapeType)y, (ColorType)x);
                }
            }
        }
    }
}

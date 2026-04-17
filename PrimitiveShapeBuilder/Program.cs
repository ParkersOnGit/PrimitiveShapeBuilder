using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace PrimitiveShapeBuilder
{
    internal class Program
    {
        internal const string VersionNumber = $"v0.1.0-alpha";

        internal static void Main(string[] args)
        {


            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(16 * 100, 9 * 100),
                Title = "Primitive Shape Builder",
                StartVisible = false,
                Vsync = VSyncMode.On,
                Icon = new WindowIcon()
            };

            using (Window window = new Window(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}

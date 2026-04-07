using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using PrimitiveShapeBuilder.Loaders;

namespace PrimitiveShapeBuilder
{
    internal class Program
    {
        internal const string VersionNumber = $"v0.0.1-alpha";

        internal static void Main(string[] args)
        {
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(16 * 100, 9 * 100),
                Title = "Primitive Shape Builder",
                StartVisible = false,
                Vsync = VSyncMode.On,
            };

            using (Window window = new Window(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}

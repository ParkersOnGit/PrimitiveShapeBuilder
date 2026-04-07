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

            Model testM = new Model();
            testM.LoadModel("../../../Assets/Models/Cube.obj");

            Console.ForegroundColor = ConsoleColor.Green;
            foreach (float f in testM.Data)
            {
                Console.WriteLine(f);
            }
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (uint i in testM.Indices)
            {
                Console.WriteLine(i);
            }

            using (Window window = new Window(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}

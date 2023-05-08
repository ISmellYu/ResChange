using System.Diagnostics;
using System.Drawing;

namespace ResChange;

internal static class Program
{
    public static void Main()
    {
        var resolution = new Resolution();
        try
        {
            const int newWidth = 1024;
            const int newHeight = 768;
            // get current resolution
            var currentResolution = Screen.PrimaryScreen.Bounds;
            Console.WriteLine("Current resolution: " + currentResolution.Width + "x" + currentResolution.Height);

            if (currentResolution is {Height: newHeight, Width: newWidth})
            {
                Console.WriteLine("Already in 1280x720");
                Console.ReadKey();
                return;
            }

            
            if (!resolution.IsDisplayModeSupported(newWidth, newHeight, out var supportedModes))
            {
                Console.WriteLine("The given resolution is not supported by your system.");
                Console.WriteLine("Supported resolutions: " + supportedModes);
                Console.ReadKey();
                return;
            }

            // change the resolution
            resolution.ChangeDisplaySettings(newWidth, newHeight);

            string path = $".\\csgo.exe -w {newWidth} -h {newHeight} -windowed -fullscreen";

            // change current directory to the game directory
            Environment.CurrentDirectory = "D:\\Steam";

            var proc = new Process();
            proc.StartInfo.FileName = "steam.exe";
            proc.StartInfo.Arguments = "-applaunch 730 -w 1280 -h 720 -freq 144 -windowed -fullscreen";
            proc.Start();

            // wait like 10 seconds to make sure the game is launched
            
            Thread.Sleep(10000);
            
            // wait for the game to close using process Process.GetProcessesByName()
            while (Process.GetProcessesByName("csgo").Length > 0)
            {
                Thread.Sleep(1000);
            }
            resolution.RestoreDisplaySettings();

        }
        catch (Exception e)
        {
            resolution.RestoreDisplaySettings();
        }
    }
}
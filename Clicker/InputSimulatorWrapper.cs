using System;
using System.Windows;
using WindowsInput;

namespace Clicker
{
    /// <summary>
    /// Static class for simulating input events
    /// </summary>
    sealed class InputSimulatorWrapper
    {
        private static InputSimulator inputSimulator = new InputSimulator();

        /// <summary>
        /// Click at normalized position. Each coordinate is between 0 and 65535
        /// </summary>
        public static void ClickAtNormalizedLocation(int x, int y)
        {
            inputSimulator.Mouse.MoveMouseTo(x, y);
            inputSimulator.Mouse.LeftButtonClick();
        }

        /// <summary>
        /// Click at position relative to screen resolution
        /// </summary>
        public static void ClickAtLocation(int x, int y)
        {
            Tuple<int, int> normalizedCoordinates = NormalizeScreenCoordinates(x, y);
            ClickAtNormalizedLocation(normalizedCoordinates.Item1, normalizedCoordinates.Item2);
        }

        private static Tuple<int, int> NormalizeScreenCoordinates(int x, int y)
        {
            const int normalizationMaximum = 65535;

            double primaryScreenWidth = SystemParameters.PrimaryScreenWidth;
            double primaryScreenHeight = SystemParameters.PrimaryScreenHeight;

            int normalizedX = (int)(x * normalizationMaximum / primaryScreenWidth);
            int normalizedY = (int)(y * normalizationMaximum / primaryScreenWidth);

            return new Tuple<int, int>(normalizedX, normalizedY);
        }

        public static void Sleep(int timeInMs)
        {
            inputSimulator.Mouse.Sleep(timeInMs);
        }
    }
}

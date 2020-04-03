using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using WindowsInput;

namespace Clicker
{
    /// <summary>
    /// Static class for simulating input events
    /// </summary>
    sealed class InputSimulatorWrapper
    {
        #region Cursor Position
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };
        #endregion

        #region Hot Keys
        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-registerhotkey?redirectedfrom=MSDN
        /// </summary>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-unregisterhotkey
        /// </summary>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        #endregion

        #region VK Codes
        public enum VK_Codes
        {
            F6 = 0x75,
            F7 = 0x76,
            F8 = 0x77
        };
        #endregion

        #region Key Modifiers
        public enum Key_Modifiers
        {
            None = 0
        };
        #endregion

        #region Win32Api Constants
        public const int HotkeyMessage = 0x0312;
        #endregion

        #region InputSimulator
        private static InputSimulator inputSimulator = new InputSimulator();
        #endregion

        #region Public Interface
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
            int normalizedY = (int)(y * normalizationMaximum / primaryScreenHeight);

            return new Tuple<int, int>(normalizedX, normalizedY);
        }

        public static Point GetMousePosition()
        {
            Win32Point w32Mouse = new Win32Point();
            bool isSuccessful = GetCursorPos(ref w32Mouse);

            if (!isSuccessful)
                throw new Exception("GetMousePosition failed Win32 API call");

            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        public static void Sleep(int timeInMs)
        {
            inputSimulator.Mouse.Sleep(timeInMs);
        }

        public static int RegisterHotKey(int id, VK_Codes virtualKey, Key_Modifiers keyModifier)
        {
            IntPtr mainWindowHandle = new WindowInteropHelper(Application.Current.MainWindow).Handle;

            uint vkCode = (uint)virtualKey;
            uint modifier = (uint)keyModifier;

            if (!RegisterHotKey(mainWindowHandle, id, modifier, vkCode))
                throw new Exception("RegisterHotKey failed Win32 API call");

            return id;
        }

        public static void UnregisterHotKey(int id)
        {
            IntPtr mainWindowHandle = new WindowInteropHelper(Application.Current.MainWindow).Handle;

            if (!UnregisterHotKey(mainWindowHandle, id))
                throw new Exception("UnregisterHotKey failed Win32 API call");
        }
        #endregion
    }
}

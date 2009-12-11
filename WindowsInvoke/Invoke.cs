using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Phrosty.IsoTool.Platform.Windows
{
    /// <summary>
    /// Static class to isolate Windows-specific methods from the rest of the solution.
    /// </summary>
    class Invoke
    {
        #region Unmanaged datastructures (used by MessangeDialog PInvokes)
        public const int PBT_APMQUERYSUSPEND = 0x0;
        public const int BROADCAST_QUERY_DENY = 0x424D5144;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        public static readonly int QueryCancelAutoPlay = RegisterWindowMessage("QueryCancelAutoPlay");
        public static readonly int WM_POWERBROADCAST = RegisterWindowMessage("WM_POWERBROADCAST");

        /// <summary>
        /// Exectuion state enum for disabling standby.
        /// </summary>
        [Flags]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001,
            ES_USER_PRESENT = 0x00000004,
        }
        #endregion

        #region MessageDialog PInvokes
        /// <summary>
        /// The send message method for allowing the window to be dragable.
        /// </summary>
        /// <param name="hWnd">The window handle.</param>
        /// <param name="msg">The message to send.</param>
        /// <param name="wParam">The wParam value.</param>
        /// <param name="lParam">The lParam value.</param>
        /// <returns>The hResult of the call.</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Releases the window caputre.
        /// </summary>
        /// <returns>True on success.</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReleaseCapture();

        /// <summary>
        /// Sets the execution state for allowing the tool to disable standby (Vista and higher).
        /// </summary>
        /// <param name="esFlags">The flags indicating the thread execution state.</param>
        /// <returns>The execution state that was set.</returns>
        [DllImport("kernel32.dll")]
        public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        /// <summary>
        /// Gets the integer value for the given window message.
        /// </summary>
        /// <param name="msgString">The window message to lookup.</param>
        /// <returns>The integer value for the message.</returns>
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int RegisterWindowMessage([In, MarshalAs(UnmanagedType.LPWStr)] string msgString);
        
        #endregion

        #region UsbDriveService PInvokes
        /// <summary>
        /// Sets the active partition.
        /// </summary>
        /// <param name="drive">The root path of the drive to update.</param>
        /// <returns>Returns 0 for success.  See System Error Codes for possible error values.</returns>
        [DllImport("IoWrapper.dll", CharSet = CharSet.Auto)]
        public static extern int SetActivePartition([In, MarshalAs(UnmanagedType.LPWStr)] string drive);

        /// <summary>
        /// Formats the drive.
        /// </summary>
        /// <param name="drive">The root path of the drive to update.</param>
        /// <returns>Returns 0 for success.  See System Error Codes for possible error values.</returns>
        [DllImport("IoWrapper.dll", CharSet = CharSet.Auto)]
        public static extern int FormatDrive([In, MarshalAs(UnmanagedType.LPWStr)] string drive);

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArtCalculator.libs
{
    public static class Win32Helper
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public static void R_HK(IntPtr hWnd, int id, int fsModifiers, int vk)
        {
            RegisterHotKey(hWnd, id, fsModifiers, vk);
        }

        public static void U_HK(IntPtr hWnd, int id)
        {
            UnregisterHotKey(hWnd, id);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

    }
}

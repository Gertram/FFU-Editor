using System;
using System.Runtime.InteropServices;

namespace FFUEditor
{
    /// <summary>
    /// This class converts a win32 bitmap to a WinFX Cursor
    /// https://johnstewien.wordpress.com/2006/06/01/wpf-system-windows-input-cursor-from-system-drawing-bitmap/
    /// </summary>
    public class WinFXCursorFromBitmap : SafeHandle
    {
        // ********************************************************************
        // Methods
        // ********************************************************************
        #region Methods
        /// <summary>
        /// Creates a WinFX cursor from a win32 bitmap
        /// </summary>
        /// <param name="cursorBitmap"></param>
        /// <returns></returns>
        public static System.Windows.Input.Cursor CreateCursor(System.Drawing.Bitmap cursorBitmap)
        {

            WinFXCursorFromBitmap csh = new WinFXCursorFromBitmap(cursorBitmap);

            return System.Windows.Interop.CursorInteropHelper.Create(csh);
        }

        /// <summary>
        /// Hidden contructor. Accessed only from the static method.
        /// </summary>
        /// <param name="cursorBitmap"></param>
        protected WinFXCursorFromBitmap(System.Drawing.Bitmap cursorBitmap): base((IntPtr)(-1), true)
        {
            handle = cursorBitmap.GetHicon();
        }

        /// <summary>
        /// Releases the bitmap handle
        /// </summary>
        /// <returns></returns>
        protected override bool ReleaseHandle()
        {
            bool result = DestroyIcon(handle);

            handle = (IntPtr)(-1);

            return result;
        }

        /// <summary>
        /// Imported from user32.dll. Destroys an icon GDI object.
        /// </summary>
        /// <param name="hIcon"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern bool DestroyIcon(IntPtr hIcon);

        #endregion Methods
        // ********************************************************************
        // Properties
        // ********************************************************************
        #region Properties

        /// <summary>
        /// Gets if the handle is valid or not
        /// </summary>
        public override bool IsInvalid => handle == (IntPtr)(-1);

        #endregion Properties
    }
}

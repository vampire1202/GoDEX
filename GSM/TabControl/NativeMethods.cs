/*
 * This code is provided under the Code Project Open Licence (CPOL)
 * See http://www.codeproject.com/info/cpol10.aspx for details
*/

using System.Drawing;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace System.Windows.Forms
{
	/// <summary>
	/// Description of NativeMethods.
	/// </summary>
	[SecurityPermission(SecurityAction.Assert, Flags=SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class NativeMethods
	{
		private NativeMethods(){}
		
#region Windows Constants

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public const int WM_GETTABRECT = 0x130a;
		public const int WS_EX_TRANSPARENT = 0x20;
		public const int WM_SETFONT = 0x30;
		public const int WM_FONTCHANGE = 0x1d;
		public const int WM_HSCROLL = 0x114;
		public const int TCM_HITTEST = 0x130D;
		public const int WM_PAINT = 0xf;
		public const int WM_ERASEBKGND = 0x0014;

#endregion

#region Content Alignment

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly ContentAlignment AnyRightAlign = ContentAlignment.BottomRight | ContentAlignment.MiddleRight | ContentAlignment.TopRight;
	    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly ContentAlignment AnyLeftAlign = ContentAlignment.BottomLeft | ContentAlignment.MiddleLeft | ContentAlignment.TopLeft;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	    public static readonly ContentAlignment AnyTopAlign = ContentAlignment.TopRight | ContentAlignment.TopCenter | ContentAlignment.TopLeft;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	    public static readonly ContentAlignment AnyBottomAlign = ContentAlignment.BottomRight | ContentAlignment.BottomCenter | ContentAlignment.BottomLeft;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
	    public static readonly ContentAlignment AnyMiddleAlign = ContentAlignment.MiddleRight | ContentAlignment.MiddleCenter | ContentAlignment.MiddleLeft;
	    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		public static readonly ContentAlignment AnyCenterAlign = ContentAlignment.BottomCenter | ContentAlignment.MiddleCenter | ContentAlignment.TopCenter;

#endregion

#region User32.dll

        [DllImport("user32.dll"), SecurityPermission(SecurityAction.Demand)]
		public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT paintStruct);
		
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT paintStruct);

#endregion

#region Misc Functions

		public static int LoWord(IntPtr dWord){
            return dWord.ToInt32() & 0xffff;
        }

        public static int HiWord(IntPtr dWord){
            if ((dWord.ToInt32() & 0x80000000) == 0x80000000)
                return (dWord.ToInt32() >> 16);
            else
                return (dWord.ToInt32() >> 16) & 0xffff;
        }
 
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2106:SecureAsserts")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
 		public static IntPtr ToIntPtr(object structure){
			IntPtr lparam = IntPtr.Zero;
			lparam = Marshal.AllocCoTaskMem(Marshal.SizeOf(structure));
			Marshal.StructureToPtr(structure, lparam, false);
			return lparam;
		}
		

#endregion

#region Windows Structures and Enums

		[Flags()]
		public enum TCHITTESTFLAGS{
			TCHT_NOWHERE = 1,
			TCHT_ONITEMICON = 2,
			TCHT_ONITEMLABEL = 4,
			TCHT_ONITEM = TCHT_ONITEMICON | TCHT_ONITEMLABEL
		}
	
		/// <summary>
		///     Specifies a raster-operation code. These codes define how the color data for the
		///     source rectangle is to be combined with the color data for the destination
		///     rectangle to achieve the final color.
		/// </summary>
		public enum TernaryRasterOperations : uint {
		    /// <summary>dest = source</summary>
		    SRCCOPY = 0x00CC0020,
		    /// <summary>dest = source OR dest</summary>
		    SRCPAINT = 0x00EE0086,
		    /// <summary>dest = source AND dest</summary>
		    SRCAND = 0x008800C6,
		    /// <summary>dest = source XOR dest</summary>
		    SRCINVERT = 0x00660046,
		    /// <summary>dest = source AND (NOT dest)</summary>
		    SRCERASE = 0x00440328,
		    /// <summary>dest = (NOT source)</summary>
		    NOTSRCCOPY = 0x00330008,
		    /// <summary>dest = (NOT src) AND (NOT dest)</summary>
		    NOTSRCERASE = 0x001100A6,
		    /// <summary>dest = (source AND pattern)</summary>
		    MERGECOPY = 0x00C000CA,
		    /// <summary>dest = (NOT source) OR dest</summary>
		    MERGEPAINT = 0x00BB0226,
		    /// <summary>dest = pattern</summary>
		    PATCOPY    = 0x00F00021,
		    /// <summary>dest = DPSnoo</summary>
		    PATPAINT = 0x00FB0A09,
		    /// <summary>dest = pattern XOR dest</summary>
		    PATINVERT = 0x005A0049,
		    /// <summary>dest = (NOT dest)</summary>
		    DSTINVERT = 0x00550009,
		    /// <summary>dest = BLACK</summary>
		    BLACKNESS = 0x00000042,
		    /// <summary>dest = WHITE</summary>
		    WHITENESS = 0x00FF0062,
		    /// <summary>
		    /// Capture window as seen on screen.  This includes layered windows 
		    /// such as WPF windows with AllowsTransparency="true"
		    /// </summary>
		    CAPTUREBLT = 0x40000000
		}	        
		
		[StructLayout(LayoutKind.Sequential)]
		public struct TCHITTESTINFO{
			
			public TCHITTESTINFO(Point location){
				pt = location;
				flags = TCHITTESTFLAGS.TCHT_ONITEM;
			}
			
			public Point pt;
			public TCHITTESTFLAGS flags;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public struct PAINTSTRUCT{
		    public IntPtr hdc;
		    public int fErase;
		    public RECT rcPaint;
		    public int fRestore;
		    public int fIncUpdate;
		    [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)] 
		    public byte[] rgbReserved;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT{
		    public int left;
		    public int top;
		    public int right;
		    public int bottom;
		    
		    public RECT(int left, int top, int right, int bottom){
		        this.left = left;
		        this.top = top;
		        this.right = right;
		        this.bottom = bottom;
		    }
		
		    public RECT(Rectangle r){
		        this.left = r.Left;
		        this.top = r.Top;
		        this.right = r.Right;
		        this.bottom = r.Bottom;
		    }
		
		    public static RECT FromXYWH(int x, int y, int width, int height){
		        return new RECT(x, y, x + width, y + height);
		    }
		
		    public static RECT FromIntPtr(IntPtr ptr){
		    	RECT rect = (RECT)Marshal.PtrToStructure(ptr, typeof(RECT));
		    	return rect;
		    }
		    
		    public Size Size{
		        get{
		            return new Size(this.right - this.left, this.bottom - this.top);
		        }
		    }
		}
		

#endregion

	}
	
}

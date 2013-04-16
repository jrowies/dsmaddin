using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.CommandBars;
using System.Drawing;
using stdole;
using System.Runtime.InteropServices;

namespace DocumentSessionManager
{
  public class PictureManager
  {
    public static Dictionary<string, CommandBarButton> CommandButtons = new Dictionary<string, CommandBarButton>();

    private const string PATH = @"c:\VSProjects\DocumentSessionManager\trunk\DocumentSessionManager\Icons2\";

    public static bool SetPicture(string command, bool enabled)
    {
      string[] commandParts = command.Split(new char[1] { '.' });
      if (commandParts.Length == 3)
        command = commandParts[2];

      CommandBarButton button;
      if (!CommandButtons.TryGetValue(command, out button))
        return false;

      Bitmap bmp;

      //todo: try to optimize...
      if (!enabled)
      {
        bmp = Image.FromFile(String.Format("{0}{1}Disabled.bmp", PATH, command)) as Bitmap;
        button.Picture = (StdPicture)GetIPictureDispFromBitmapHandle(bmp.GetHbitmap());

      }
      else
      {
        bmp = Image.FromFile(String.Format("{0}{1}.bmp", PATH, command)) as Bitmap;
        button.Picture = (StdPicture)GetIPictureDispFromBitmapHandle(bmp.GetHbitmap());
      }

      bmp = Image.FromFile(String.Format("{0}{1}Mask.bmp", PATH, command)) as Bitmap;
      button.Mask = (StdPicture)GetIPictureDispFromBitmapHandle(bmp.GetHbitmap());

      return true;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr LoadBitmap(IntPtr hInstance, string lpBitmapName);

    [DllImport("olepro32.dll")]
    private static extern int OleCreatePictureIndirect(ref PICTDESC pPictDesc, ref Guid riid, int fOwn, [MarshalAs(UnmanagedType.IDispatch)] ref object ppvObj);

    private static IPictureDisp GetIPictureDispFromBitmapHandle(IntPtr hBitmapHandle)
    {
      object objPicture = null;
      Guid objGuid = new Guid("00020400-0000-0000-C000-000000000046");
      int iResult;
      PICTDESC tPICTDESC = new PICTDESC(hBitmapHandle);

      iResult = OleCreatePictureIndirect(ref tPICTDESC, ref objGuid, 1, ref objPicture);

      return (stdole.IPictureDisp)objPicture;
    }
  }

  internal struct PICTDESC
  {
    public int SizeOfStruct;
    public int PicType;
    public IntPtr Hbitmap;
    public IntPtr Hpal;
    public int Padding;
    public PICTDESC(IntPtr hBmp)
    {
      SizeOfStruct = Marshal.SizeOf(typeof(PICTDESC));
      PicType = 1;
      Hbitmap = hBmp;
      Hpal = IntPtr.Zero;
      Padding = 0;
    }
  }
}

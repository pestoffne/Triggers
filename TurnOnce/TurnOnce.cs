using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

static unsafe class TurnOnce
{
  [DllImport("kernel32.dll")]
  static extern IntPtr GetConsoleWindow();

  [DllImport("user32.dll")]
  static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

  static void Hide()
  {
    IntPtr hWnd = GetConsoleWindow();
    ShowWindow(hWnd, 0);
  }

  [DllImport("user32.dll")]
  static extern void mouse_event(
    UInt32 dwFlags,
    UInt32 dx,
    UInt32 dy,
    UInt32 dwData,
    UInt64 *dwExtraInfo
  );

  const UInt32 MOUSEEVENTF_MOVE      = 0x0001;
  const UInt32 MOUSEEVENTF_LEFTDOWN  = 0x0002;
  const UInt32 MOUSEEVENTF_LEFTUP    = 0x0004;
  const UInt32 MOUSEEVENTF_RIGHTDOWN = 0x0008;
  const UInt32 MOUSEEVENTF_RIGHTUP   = 0x0010;
  const UInt32 MOUSEEVENTF_ABSOLUTE  = 0x8000;

  static void MouseRightDown()
  {
    UInt64 dwExtraInfo = 0;
    mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, &dwExtraInfo);
  }

  static void MouseRightUp()
  {
    UInt64 dwExtraInfo = 0;
    mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, &dwExtraInfo);
  }

  static void Turn(int pixels)
  {
    Point temp = Cursor.Position;
    Cursor.Position = new Point(0, 40);
    Thread.Sleep(10);
    Cursor.Position = new Point(0, 40);
    MouseRightDown();
    Thread.Sleep(10);
    Cursor.Position = new Point(pixels, 40);
    Thread.Sleep(10);
    Cursor.Position = new Point(pixels, 40);
    MouseRightUp();
    Thread.Sleep(10);
    Cursor.Position = temp;
  }

  static void Main(string[] argv)
  {
    Hide();
    Turn(500);
  }
}

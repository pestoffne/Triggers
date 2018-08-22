using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

static unsafe class TurnKey
{
  [DllImport("user32.dll")]
  static extern short GetKeyState(Keys k);

  static bool IsPressed(Keys k)
  {
    return GetKeyState(k) < 0;
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

  static Keys toKey(string key)
  {
    var strToKey = new Dictionary<string, Keys>()
    {
        {"A", Keys.A},
        {"B", Keys.B},
        {"C", Keys.C},
        {"D", Keys.D},
        {"E", Keys.E},
        {"F", Keys.F},
        {"G", Keys.G},
        {"H", Keys.H},
        {"I", Keys.I},
        {"J", Keys.J},
        {"K", Keys.K},
        {"L", Keys.L},
        {"M", Keys.M},
        {"N", Keys.N},
        {"O", Keys.O},
        {"P", Keys.P},
        {"Q", Keys.Q},
        {"R", Keys.R},
        {"S", Keys.S},
        {"T", Keys.T},
        {"U", Keys.U},
        {"V", Keys.V},
        {"W", Keys.W},
        {"X", Keys.X},
        {"Y", Keys.Y},
        {"Z", Keys.Z},

        {"0", Keys.D0},
        {"1", Keys.D1},
        {"2", Keys.D2},
        {"3", Keys.D3},
        {"4", Keys.D4},
        {"5", Keys.D5},
        {"6", Keys.D6},
        {"7", Keys.D7},
        {"8", Keys.D8},
        {"9", Keys.D9},

        {"`", Keys.Oemtilde},
        {"[", Keys.OemOpenBrackets},
        {"]", Keys.OemCloseBrackets},
        {";", Keys.OemSemicolon},
        {"'", Keys.OemQuotes},
        {",", Keys.Oemcomma},
        {".", Keys.OemPeriod},
        {"/", Keys.Divide},
    };
    
    return strToKey[key];
  }

  static DateTime last_turn_time = new DateTime(0);
  static TimeSpan one_second = new TimeSpan(TimeSpan.TicksPerSecond);

  static void ScanKeyboard(int pixels, Keys key)
  {
    if (IsPressed(key))
    {
      DateTime current_time = DateTime.Now;

      if (current_time - last_turn_time > one_second)
      {
        Turn(pixels);
        last_turn_time = current_time;
        Console.WriteLine("�������� ������� {0}", current_time);
      }
    }
  }

  static void Main(string[] argv)
  {
    int pixels;
    Keys key;

    try
    {
      string config = File.ReadAllText("Configuration.txt");
      string[] elements = config.Split(' ', '\r', '\n');
      pixels = Convert.ToInt32(elements[0], 10);
      key = toKey(elements[1]);
    }
    catch (Exception e)
    {
      Console.WriteLine("Excetion: {0}", e);
      Console.ReadLine();
      return;
    }

    Console.WriteLine("����� ������� �� {0} ��������.", pixels);
    Console.WriteLine("��������� �������� ������� \"{0}\".", key);
    Console.WriteLine("������ ������ {0}", DateTime.Now);
    
    const int sleep_ms = 10;

    while (true)
    {
      Thread.Sleep(sleep_ms);
      ScanKeyboard(pixels, key);
    }
  }
}

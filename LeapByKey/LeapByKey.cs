using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

static class Application
{
  [DllImport("user32.dll")]
  static extern short GetKeyState(Keys k);

  static bool IsPressed(Keys k)
  {
    return GetKeyState(k) < 0;
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
        Triggers.Leap(pixels);
        last_turn_time = current_time;
        Console.WriteLine("Триггер выполнен {0}", current_time);
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

    Console.WriteLine("Выполнение Triggers.Leap({0}) при нажатии на кнопку {1}, не чаще чем раз в секунду.", pixels, key);
    Console.WriteLine("Начало работы {0}", DateTime.Now);
    
    const int sleep_ms = 10;

    while (true)
    {
      Thread.Sleep(sleep_ms);
      ScanKeyboard(pixels, key);
    }
  }
}

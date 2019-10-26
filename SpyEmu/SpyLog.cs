using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SpyEmu
{
    public class SpyLog
    {
        private static string LOG_REGEX_PATTERN { get; } =
            @"<(?<No>\d+)> (?<hWnd>\w+) (\w) " +
            @"(?<nCode>WM_\w+) " +
            @"wParam:(?<wParam>\w+) " +
            @"lParam:(?<lParam>\w+) " +
            @"time:(?<time>[\d:\.]+)";
        private static Regex LogRegex { get; } = new Regex(LOG_REGEX_PATTERN);
        private static string TIME_REGEX_PATTERN { get; } =
            @"(?<Hour>\d+):(?<Minute>\d+):(?<Second>\d+)\.(?<Millisecond>\d+)";
        private static Regex TimeRegex { get; } = new Regex(TIME_REGEX_PATTERN);

        public int No { get; set; }
        public IntPtr hWnd { get; set; }
        public uint nCode { get; set; }
        public string nCodeStr { get; set; }
        public IntPtr wParam { get; set; }
        public IntPtr lParam { get; set; }
        public DateTime Time { get; set; }

        public static bool IsMatch(string line)
        {
            Match m = LogRegex.Match(line);
            if (m.Length == 0)
            {
                return false;
            }

            string nCode = m.Groups["nCode"].Value;
            return Code.Contains(nCode);
        }

        public static SpyLog Parse(string line)
        {
            Match m = LogRegex.Match(line);
            string no = m.Groups["No"].Value;
            string hWnd = m.Groups["hWnd"].Value;
            string nCode = m.Groups["nCode"].Value;
            string wParam = m.Groups["wParam"].Value;
            string lParam = m.Groups["lParam"].Value;
            string timeStr = m.Groups["time"].Value;

            Match mTime = TimeRegex.Match(timeStr);
            int year = 2000;
            int month = 1;
            int day = 1;
            string hourStr = mTime.Groups["Hour"].Value.PadLeft(2, '0');
            string minStr = mTime.Groups["Minute"].Value.PadLeft(2, '0');
            string secStr = mTime.Groups["Second"].Value.PadLeft(2, '0');
            string miliSecStr = mTime.Groups["Millisecond"].Value.PadLeft(3, '0');
            int hms = int.Parse(hourStr + minStr + secStr + miliSecStr);
            if (240000000 < hms)
            {
                day += 1;
                hourStr = (int.Parse(hourStr) - 24).ToString().PadLeft(2, '0');
            }
            int hour = int.Parse(hourStr);
            int min = int.Parse(minStr);
            int sec = int.Parse(secStr);
            int milisec = int.Parse(miliSecStr);
            DateTime time = new DateTime(year, month, day, hour, min, sec, milisec);

            try
            {
                return new SpyLog
                {
                    No = int.Parse(no),
                    hWnd = ParseHexNumber(hWnd),
                    nCode = Code.Parse(nCode),
                    nCodeStr = nCode,
                    wParam = ParseHexNumber(wParam),
                    lParam = ParseHexNumber(lParam),
                    Time = time,
                };
            }
            catch (Exception e)
            {
                Log.Write(e);

                return new SpyLog
                {
                    No = -1,
                    hWnd = IntPtr.Zero,
                    nCode = 0u,
                    nCodeStr = string.Empty,
                    wParam = IntPtr.Zero,
                    lParam = IntPtr.Zero,
                    Time = DateTime.MinValue,
                };
            }
        }

        private static IntPtr ParseHexNumber(string hexNumber)
        {
            return new IntPtr(int.Parse(hexNumber, NumberStyles.HexNumber));
        }

        public override string ToString()
        {
            return string.Format("{0}, wParam: {1}, lParam: {2}", nCodeStr, wParam, lParam);
        }
    }
}

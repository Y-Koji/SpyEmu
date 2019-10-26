using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpyEmu
{
    public static class Spy
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        delegate bool EnumWindowsProcDelegate(IntPtr windowHandle, IntPtr lParam);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetWindow(IntPtr hWnd, GetWindowType uCmd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        /// <summary>ウインドウクラス名取得</summary>
        /// <param name="hWnd">ウインドウハンドル</param>
        /// <returns>クラス名</returns>
        public static string GetClassName(IntPtr hWnd)
        {
            StringBuilder className = new StringBuilder(256);
            int nRet = GetClassName(hWnd, className, className.Capacity);

            if (0 != nRet)
            {
                return className.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>Spy++ログファイルからプロセス、クラスパスを取得</summary>
        /// <param name="fileName">Spy++ログファイル</param>
        /// <returns>(Process: プロセス, string: クラスパス)</returns>
        public static (Process p, string path) GetInfo(string fileName)
        {
            string REG_PROCESS = @"(?<Name>\w+)+\[(?<Index>\d+)\]";
            string line = File.ReadAllLines(fileName).First();
            string[] split = line.Split(',');

            Match mProcess = Regex.Match(split[0], REG_PROCESS);
            string name = mProcess.Groups["Name"].Value;
            int index = int.Parse(mProcess.Groups["Index"].Value);
            Process p = Process.GetProcessesByName(name)[index];

            return (p, split[1]);
        }

        /// <summary>Spy++ログファイルからログ一覧を取得</summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static IEnumerable<SpyLog> GetLog(string fileName)
        {
            return
                from x in 
                    from x in File.ReadAllLines(fileName)
                    where !string.IsNullOrWhiteSpace(x)
                    where SpyLog.IsMatch(x)
                    select SpyLog.Parse(x)
                where x.No != -1
                orderby x.Time
                select x;
        }

        /// <summary>子ウインドウ一覧情報取得</summary>
        /// <param name="hWnd">親ウインドウハンドル</param>
        /// <returns>ウインドウ一覧情報</returns>
        public static IEnumerable<Window> FindChildWindows(IntPtr hWnd)
            => FindWindows(GetWindow(hWnd, GetWindowType.GW_CHILD));

        /// <summary>ウインドウ一覧情報取得</summary>
        /// <param name="hWnd">検索元ウインドウハンドル</param>
        /// <param name="path">検索元ウインドウハンドルのパス</param>
        /// <returns>ウインドウ一覧情報</returns>
        public static IEnumerable<Window> FindWindows(IntPtr hWnd, string path = "")
        {
            if (IntPtr.Zero == hWnd)
            {
                goto EXIT;
            }

            IDictionary<string, int> indexer = new Dictionary<string, int>();
            do
            {
                string className = GetClassName(hWnd);
                if (!indexer.ContainsKey(className))
                {
                    indexer.Add(className, 0);
                }
                else
                {
                    indexer[className]++;
                }
                string classNameWithIndex = className + "[" + indexer[className] + "]";
                
                yield return new Window
                {
                    Path = path + classNameWithIndex,
                    ClassName = className,
                    hWnd = hWnd,
                };

                string nextPath = path + classNameWithIndex + "/";
                foreach (var child in FindWindows(GetWindow(hWnd, GetWindowType.GW_CHILD), nextPath))
                {
                    yield return child;
                }
            } while ((hWnd = GetWindow(hWnd, GetWindowType.GW_HWNDNEXT)) != IntPtr.Zero);

            EXIT:;
        }

        /// <summary>指定ウインドウにメッセージログを投げ、動作をシミュレートする</summary>
        /// <param name="hWnd">ウインドウハンドル</param>
        /// <param name="logs">ログ一覧</param>
        public static void PostLogs(IntPtr hWnd, IEnumerable<SpyLog> logs)
        {
            SpyLog[] logArray = logs.ToArray();
            for (int i = 0;i < logArray.Length;i++)
            {
                SpyLog nowLog = logArray[i];
                SpyLog beforeLog = 0 < i ? logArray[i - 1] : logArray[i];
                TimeSpan ts = nowLog.Time - beforeLog.Time;

                Task.Delay((int)ts.TotalMilliseconds).Wait();

                PostMessage(hWnd, nowLog.nCode, nowLog.wParam, nowLog.lParam);

                Console.WriteLine(nowLog.ToString());
            }
        }
    }
}

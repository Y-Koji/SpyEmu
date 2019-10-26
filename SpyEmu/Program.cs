using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SpyEmu
{
    /// <summary>
    /// Main Program.
    /// 引数に指定されたSpy++ログファイルを読み込み、
    /// ログファイル内の1行目に記載されているプロセス名、ウインドウクラスパス
    /// へログメッセージを投げて動作をシミュレートする。
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // テスト用にmeryのログを使用する。
            // 完成後はこの行を外す。
            args = new string[] { @"C:\Users\PC user\Desktop\SpyLog\mery.log", };

            // シミュレート処理
            foreach (var arg in args)
            {
                // ログファイルが無ければ処理しない
                if (!File.Exists(arg))
                {
                    continue;
                }

                // ログファイルから実行中のプロセス、
                // ウインドウクラスのパスを取得する。
                (Process p, string path) = Spy.GetInfo(arg);

                // ログファイルからシミュレート可能ログ一覧を取得する。
                // ※ シミュレート可能ログは「Code.cs」コメント参照
                var logs = Spy.GetLog(arg).ToArray();
                
                // プロセスからウインドウハンドル一覧を取得する
                var windows = Spy.FindChildWindows(p.MainWindowHandle).ToArray();

                // ウインドウハンドル一覧から、ログファイルに記載されている
                // ウインドウクラスパスのウインドウ情報を取得する。
                var window = windows.Single(x => x.Path == path);

                // 取得したウインドウへログのメッセージを投げ、動作をシミュレートする。
                Spy.PostLogs(window.hWnd, logs);
            }
        }
    }
}

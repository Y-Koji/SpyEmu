using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpyEmu
{
    /// <summary>ログ出力クラス</summary>
    public static class Log
    {
        /// <summary>例外のログ書き込み</summary>
        /// <param name="e">例外オブジェクト</param>
        /// <param name="filePath"></param>
        /// <param name="name"></param>
        /// <param name="line"></param>
        public static void Write(Exception e,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string name = "",
            [CallerLineNumber] int line = 0)
        {
            string msg =
                string.Format("{0}#{1}:{2}\r\n{3}\r\n{4}\r\n",
                filePath, name, line, e.Message, e.StackTrace);

            Console.WriteLine(msg);
        }
    }
}

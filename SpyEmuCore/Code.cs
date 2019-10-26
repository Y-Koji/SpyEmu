using System.Collections.Generic;

namespace SpyEmuCore
{
    /// <summary>
    /// ウインドウメッセージ⇒コード変換クラス
    /// ※ 本クラスのnCodeDictで定義されているメッセージのみ利用できる
    ///    エミュレートしたいメッセージを増やす際には、このクラスに追記する。
    /// </summary>
    public static class Code
    {
        private static IDictionary<string, uint> nCodeDict = new Dictionary<string, uint>
        {
            //{ "WM_KEYFIRST", 0x0100 },
            { "WM_KEYDOWN", 0x0100 },
            { "WM_KEYUP", 0x0101 },
            //{ "WM_CHAR", 0x0102 },
            //{ "WM_DEADCHAR", 0x0103 },
            //{ "WM_SYSKEYDOWN", 0x0104 },
            //{ "WM_SYSKEYUP", 0x0105 },
            //{ "WM_SYSCHAR", 0x0106 },
            //{ "WM_SYSDEADCHAR", 0x0107 },
            //{ "WM_CTLCOLORMSGBOX", 0x0132 },
            //{ "WM_CTLCOLOREDIT", 0x0133 },
            //{ "WM_CTLCOLORLISTBOX", 0x0134 },
            //{ "WM_CTLCOLORBTN", 0x0135 },
            //{ "WM_CTLCOLORDLG", 0x0136 },
            //{ "WM_CTLCOLORSCROLLBAR", 0x0137 },
            //{ "WM_CTLCOLORSTATIC", 0x0138 },
            //{ "MN_GETHMENU", 0x01E1 },
            { "WM_MOUSEFIRST", 0x0200 },
            { "WM_MOUSEMOVE", 0x0200 },
            { "WM_LBUTTONDOWN", 0x0201 },
            { "WM_LBUTTONUP", 0x0202 },
            { "WM_LBUTTONDBLCLK", 0x0203 },
            { "WM_RBUTTONDOWN", 0x0204 },
            { "WM_RBUTTONUP", 0x0205 },
            { "WM_RBUTTONDBLCLK", 0x0206 },
            { "WM_MBUTTONDOWN", 0x0207 },
            { "WM_MBUTTONUP", 0x0208 },
            { "WM_MBUTTONDBLCLK", 0x0209 },
            { "WM_XBUTTONDOWN", 0x020B },
            { "WM_XBUTTONUP", 0x020C },
            { "WM_XBUTTONDBLCLK", 0x020D },
            { "WM_MOUSEHWHEEL", 0x020E },
        };

        public static bool Contains(string nCode) =>
            nCodeDict.ContainsKey(nCode);

        public static uint Parse(string nCode)
            => nCodeDict[nCode];
    }
}

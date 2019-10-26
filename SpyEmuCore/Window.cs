using System;

namespace SpyEmuCore
{
    public class Window
    {
        public string Path { get; set; }
        public IntPtr hWnd { get; set; }
        public string ClassName { get; set; }

        public override string ToString()
        {
            return Path;
        }
    }
}

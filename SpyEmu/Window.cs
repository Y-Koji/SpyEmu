using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyEmu
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpyEmuCore;

namespace WindowView.Model
{
    public class WindowModel
    {
        public Process Process { get; set; } = null;
        public Window Window { get; set; }
    }
}

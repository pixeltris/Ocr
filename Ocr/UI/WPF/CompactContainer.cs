using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Ocr.UI
{
    class CompactContainer : Grid
    {
    }

    class CompactContentContainer : DockPanel
    {
        public WPFCompactForm Window { get; private set; }

        public CompactContentContainer(WPFCompactForm window)
        {
            Window = window;
        }
    }
}

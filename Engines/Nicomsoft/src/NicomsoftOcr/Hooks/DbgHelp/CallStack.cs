using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Huuk.DbgHelp
{
    class CallStack
    {
        public List<StackFrame> Frames { get; private set; }

        public int ReturnAddress
        {
            get
            {
                if (Frames != null && Frames.Count > 0)
                    return Frames[0].Address;
                return 0;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
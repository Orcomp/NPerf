using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerf.Framework.Interfaces
{
    public interface IPerfCounter
    {
        double Value { get; set; }
    }
}

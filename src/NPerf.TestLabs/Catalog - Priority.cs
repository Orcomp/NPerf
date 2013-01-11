using System.Collections.Generic;
using DaveSexton.Labs;

namespace NPerf.TestLabs
{
    internal sealed partial class Catalog : LabCatalog
    {
        /* Priority Labs (optional)
         * 
         * Specify the labs that you are currently working on and they will be 
         * executed before the other labs in this project. The remaining labs 
         * will be discovered automatically by MEF composition.  The anonymous
         * lab, if enabled, is always executed before the labs specified here.
         */
        private static IEnumerable<ILab> GetPriorityLabs()
        {
            yield break;
        }
    }
}
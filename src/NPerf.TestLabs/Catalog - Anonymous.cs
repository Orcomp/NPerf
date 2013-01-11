using System;
using System.Collections.Generic;
using System.Linq;
using DaveSexton.Labs;

namespace NPerf.TestLabs
{
    internal sealed partial class Catalog : LabCatalog
    {
        /* Enable this anonymous lab by setting the following field 
         * to true and it will be executed before all of the priority 
         * labs and discovered labs.
         */
        private const bool anonymousEnabled = false;

        private static void Anonymous()
        {
            LabTraceSource.Default.TraceLine("Hello from an anonymous lab.");
        }
    }
}
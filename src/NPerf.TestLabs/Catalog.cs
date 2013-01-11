using System;
using System.Collections.Generic;
using System.Linq;
using DaveSexton.Labs;

namespace NPerf.TestLabs
{
    internal sealed partial class Catalog : LabCatalog
    {
        public override LabActivationStrategies Activation
        {
            get
            {
                return LabActivationStrategies.AllWithExclusions;
            }
        }

        public override IEnumerable<Type> LabTypes
        {
            get
            {
                // Yield lab types that must not be activated.
                yield break;
            }
        }

        public override IEnumerable<ILab> PriorityLabs
        {
            get
            {
                /* Catalog - Anonymous.cs
                 * 
                 * The anonymous lab is executed first when enabled.
                 */
                yield return new AnonymousLab(enabled: anonymousEnabled, main: Anonymous);

                /* Catalog - Priority.cs
                 * 
                 * Labs specified here will not be executed again when they are discovered by MEF.
                 */
                foreach (var lab in GetPriorityLabs() ?? Enumerable.Empty<ILab>())
                {
                    yield return lab;
                }
            }
        }
    }
}
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("NPerf.Lab")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("NPerf.Lab")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

#if DEBUG
[assembly: InternalsVisibleTo("NPerf.Test.Lab")]
[assembly: InternalsVisibleTo("NPerf.Test.Helpers")]
#endif

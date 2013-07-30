using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerf.Core.Tools
{
    using System.IO;
    using System.Reflection;

    public class AssemblyResolver
    {
        private static readonly IList<DirectoryInfo> directories = new List<DirectoryInfo>();

        static AssemblyResolver()
        {
            directories.Add(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory);
        }

        public static void Loaded(object sender, AssemblyLoadEventArgs args)
        {
            var dir = new FileInfo(args.LoadedAssembly.Location).Directory;
            if (!directories.Contains(dir))
            {
                directories.Add(dir);
            }
        }

        public static Assembly Resolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name);

            var assemblyFiles = (from dir in directories
                                 from file in dir.GetFiles("*.dll")
                                 where file.Name == string.Format("{0}.dll", assemblyName.Name)
                                 select file).ToArray();

            if (assemblyFiles.Length == 0)
            {                
                return null;
            }

            if (assemblyFiles.Length == 1)
            {
                return Assembly.LoadFile(assemblyFiles[0].FullName);
            }

            return
                assemblyFiles.Select(assemblyFile => Assembly.LoadFile(assemblyFile.FullName))
                             .FirstOrDefault(assm => assm.GetName().Equals(assemblyName));
        }

    }
}

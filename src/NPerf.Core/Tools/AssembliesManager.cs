using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerf.Core.Tools
{
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    public class AssembliesManager
    {
        private static readonly IList<string> directories = new List<string>();
        private static readonly IList<Assembly> loadedAssemblies = new List<Assembly>();

        static AssembliesManager()
        {            
            directories.Add(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName);
        }

        public static Assembly LoadAssembly(string fileName)
        {
            var result =
                (from assembly in loadedAssemblies where assembly.Location == fileName select assembly).FirstOrDefault();
            if (result != null)
            {
                return result;
            }
           
            return Assembly.LoadFrom(fileName);
        }

        public static Type LoadType(string fileName, string typeName)
        {
            return LoadAssembly(fileName).GetTypes().First(t => t.Name.Equals(typeName));
        }

        public static void Loaded(object sender, AssemblyLoadEventArgs args)
        {
            loadedAssemblies.Add(args.LoadedAssembly);

            var dir = new FileInfo(args.LoadedAssembly.Location).Directory;
            if (!directories.Contains(dir.FullName))
            {
                directories.Add(dir.FullName);
            }
        }

        public static Assembly Resolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name);

            if (assemblyName.Name.EndsWith(".resources") && assemblyName.CultureInfo.Name == "neutral")
            {
                return null;
            }

            var loaded = (from assembly in loadedAssemblies
                          where assembly.FullName == assemblyName.FullName
                          select assembly).FirstOrDefault();

            if (loaded != null)
            {
                return loaded;
            }

            var assemblyFiles = (from dirName in directories
                                 let dir = new DirectoryInfo(dirName)
                                 from file in dir.GetFiles("*.dll", SearchOption.AllDirectories)
                                 where file.Name == string.Format("{0}.dll", assemblyName.Name)
                                 select file).ToArray();

            if (assemblyFiles.Length == 0)
            {
                return null;
            }

            if (assemblyFiles.Length == 1)
            {
                return AssembliesManager.LoadAssembly(assemblyFiles[0].FullName);
            }

            return
                assemblyFiles.Select(assemblyFile => AssembliesManager.LoadAssembly(assemblyFile.FullName))
                             .FirstOrDefault(assm => assm.GetName().FullName == assemblyName.FullName);

        }

    }
}

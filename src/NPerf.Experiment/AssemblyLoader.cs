namespace NPerf.Experiment
{
    using System;
    using System.Linq;
    using System.Reflection;

    using NPerf.Core.Tools;

    internal class AssemblyLoader
    {
        public static T CreateInstance<T>(string suiteAssemblyName, string suiteTypeName)
        {
            if (string.IsNullOrWhiteSpace(suiteAssemblyName))
            {
                throw new ArgumentNullException("suiteAssemblyName");
            }

            if (string.IsNullOrWhiteSpace(suiteTypeName))
            {
                throw new ArgumentNullException("suiteTypeName");
            }
            
            var instance = CreateInstance(suiteAssemblyName, suiteTypeName);
            return instance == null ? default(T) : (T)instance;
        }

        public static object CreateInstance(string assemblyName, string typeName)
        {
            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }

            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentNullException("typeName");
            }

            var type = AssembliesManager.LoadType(assemblyName, typeName);
            var instance = type == null ? null : Activator.CreateInstance(type);
            if (instance != null)
            {
                Console.WriteLine(@"Instance of type {0} was created.", typeName);
            }
            else
            {
                Console.Error.WriteLine(@"Instance of type {0} was not created.", typeName);
            }

            return instance;
        }

        /// <summary>
        /// This method is needed to create an instance of a generic type using the
        /// fully qualified name.
        /// <see cref="http://msdn.microsoft.com/en-us/library/w3f99sx1.aspx"/>
        /// </summary>
        /// <param name="assemblyName">The name of the assembly</param>
        /// <param name="typeName">The full name of the type</param>
        /// <returns>A new instance of the specified type</returns>
        public static object CreateInstanceFullyQualifiedName(string assemblyName, string typeName)
        {
            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }

            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentNullException("typeName");
            }

            var assembly = AssembliesManager.LoadAssembly(assemblyName);
            var type = Type.GetType(typeName + "," + assembly.FullName);

            var instance = type == null ? null : Activator.CreateInstance(type);
            if (instance != null)
            {
                Console.WriteLine(@"Instance of type {0} was created.", typeName);
            }
            else
            {
                Console.Error.WriteLine(@"Instance of type {0} was not created.", typeName);
            }

            return instance;
        }
    }
}

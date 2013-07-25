namespace NPerf.Experiment
{
    using System;
    using System.Linq;
    using System.Reflection;

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

            var type = LoadType(assemblyName, typeName);
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

        private static Type LoadType(string assemblyName, string typeName)
        {
            var assembly = Assembly.LoadFrom(assemblyName);
            Console.WriteLine(@"Assembly {0} loaded", assembly);
            var type = assembly.GetTypes().FirstOrDefault(t => t.Name.Equals(typeName));

            if (type != null)
            {
                Console.WriteLine(@"Type {0} finded.", type);
            }
            else
            {
                Console.Error.WriteLine(@"Type with name {0} not found.", typeName);
            }

            return type;
        }
    }
}

namespace NPerf.Experiment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class UsefulExtensions
    {
        public static IList<Argument> ConvertToArguments(this string[] args)
        {
            args = (" " + string.Join(" ", args).Trim()).Split(new[] { " -" }, StringSplitOptions.None);
            if (args.Length == 0)
            {
                return new Argument[0];
            }

            if (!string.IsNullOrWhiteSpace(args[0]))
            {
                throw new ArgumentException("The argumens has incorrect format.");
            }

            return (from arg in args
                    where !string.IsNullOrWhiteSpace(arg)
                    let spaceIdx = arg.IndexOf(' ')
                    select
                        new Argument
                            {
                                Name = (spaceIdx == -1) ? arg : arg.Substring(0, spaceIdx),
                                Value = (spaceIdx == -1) ? string.Empty : arg.Substring(spaceIdx).Trim()
                            }).ToList();
        }

        public static string ExtractValue(this IList<Argument> arguments, string name)
        {
            var arg = arguments.FirstOrDefault(x => x.Name.Equals(name));
            Console.WriteLine(
                @"Extracted value: ""{0}"" for argument parameter name: ""{1}""", arg == null ? "null" : arg.Value, name);
            if (arg != null)
            {
                arguments.Remove(arg);
                return arg.Value;
            }

            return null;
        }
    }
}

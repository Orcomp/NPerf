/*
* Arguments class: application arguments interpreter
*
* Authors:		R. LOPES
* Contributors:	R. LOPES
* Created:		25 October 2002
* Modified:		28 October 2002
*
* Version:		1.0
*/

namespace NPerf.Cons
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Arguments class
    /// </summary>
    public class ClArguments
    {
        // Variables
        private IDictionary<string, ClParameter> parameters;

        private StringDictionary uniqueParameters;

        private IDictionary<string, StringCollection> duplicateParameters;

        public ClArguments()
        {
            this.parameters = new Dictionary<string, ClParameter>();
            this.uniqueParameters = new StringDictionary();
            this.duplicateParameters = new Dictionary<string, StringCollection>();

            this.AddParameter(new ClParameter("help", "h", "Show this help", true, true));
        }

        public void AddParameter(ClParameter param)
        {
            this.parameters.Add(param.ShortName, param);
        }

        public string ToHelpString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Command Line Parameter Help");
            sb.AppendLine("---------------------------");
            sb.AppendLine("Parameters can be called by setting -- with their full name,");
            sb.AppendLine("or - and their short name:");
            sb.AppendLine("   --help or -h");
            sb.AppendLine("Parameters can be followed by a value:");
            sb.AppendLine("   --input data");
            sb.AppendLine();
            sb.AppendLine("Name\tShortName\tDescription\tUnique\tAlone");
            foreach (var param in this.parameters)
            {
                sb.AppendLine(param.Value.ToHelpString());
            }

            return sb.ToString();
        }

        public void Parse(string[] Args)
        {
            try
            {
                var spliter = new Regex(@"^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                var remover = new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                string parameter = null;

                foreach (var parts in Args.Select(txt => spliter.Split(txt, 3)))
                {
                    switch (parts.Length)
                    {
                            // Found a value (for the last parameter found (space separator))
                        case 1:
                            if (parameter != null)
                            {
                                if (this.CheckParameter(parameter))
                                {
                                    parts[0] = remover.Replace(parts[0], "$1");
                                    this.AddPair(parameter, parts[0]);
                                }

                                parameter = null;
                            }

                            // else Error: no parameter waiting for a value (skipped)
                            break;

                            // Found just a parameter
                        case 2:
                            // The last parameter is still waiting. With no value, set it to true.
                            if (parameter != null)
                            {
                                if (this.CheckParameter(parameter))
                                {
                                    // add to unique, alone parameters
                                    this.AddUnique(parameter);
                                }
                            }
                            parameter = parts[1];
                            break;

                            // Parameter with enclosed value
                        case 3:
                            // The last parameter is still waiting. With no value, set it to true.
                            if (parameter != null)
                            {
                                this.AddUnique(parameter);
                            }

                            parameter = parts[1];

                            // Remove possible enclosing characters (",')
                            if (this.CheckParameter(parameter))
                            {
                                parts[2] = remover.Replace(parts[2], "$1");
                                this.AddPair(parameter, parts[2]);
                            }

                            parameter = null;
                            break;
                    }
                }

                // In case a parameter is still waiting
                if (parameter != null && this.CheckParameter(parameter))
                {
                    this.AddUnique(parameter);
                }
            }
            catch (Exception)
            {
                Console.WriteLine(this.ToHelpString());
                throw;
            }
        }

        internal void AddPair(string key, string value)
        {
            var p = this.parameters[key];
            if (p.IsUnique)
            {
                if (this.uniqueParameters.ContainsKey(key))
                {
                    throw new Exception("Duplicate unique argument " + key);
                }

                this.uniqueParameters.Add(p.Name, value.TrimEnd(' '));
                this.uniqueParameters.Add(p.ShortName, value.TrimEnd(' '));
            }
            else
            {
                if (!this.duplicateParameters.ContainsKey(key))
                {
                    this.duplicateParameters.Add(p.Name, new StringCollection());
                    this.duplicateParameters.Add(p.ShortName, new StringCollection());
                }

                this.duplicateParameters[p.Name].Add(value.TrimEnd(' '));
                this.duplicateParameters[p.ShortName].Add(value.TrimEnd(' '));
            }
        }

        internal void AddUnique(string key)
        {
            if (this.uniqueParameters.ContainsKey(key))
            {
                Console.WriteLine("Duplicate parameter: {0}", key);
            }
            else
            {
                var p = this.parameters[key];
                this.uniqueParameters.Add(p.Name, string.Empty);
                this.uniqueParameters.Add(p.ShortName, string.Empty);
            }
        }

        internal bool CheckParameter(string key)
        {
            if (!this.parameters.ContainsKey(key))
            {
                Console.WriteLine("Unknown parameter: {0}", key);
                return false;
            }

            return true;
        }

        // Retrieve a parameter value if it exists
        public bool ContainsUnique(string key)
        {
            return this.uniqueParameters.ContainsKey(key);
        }

        public bool ContainsDuplicate(string key)
        {
            return this.duplicateParameters.ContainsKey(key);
        }

        public String Unique(string key)
        {
            return this.uniqueParameters[key];
        }

        public StringCollection Duplicates(string key)
        {
            return this.duplicateParameters[key];
        }

        public void ProcessHelp()
        {
            if (this.uniqueParameters.ContainsKey("h"))
            {
                Console.WriteLine(this.ToHelpString());
            }
        }
    }
}


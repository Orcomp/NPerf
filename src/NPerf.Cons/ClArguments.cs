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

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.IO;

namespace NPerf.Cons
{
	using NPerf.Cons.Collections;

	public class ClParameter
	{
		private string name;
		private string shortName;
		private string description;
		private bool unique;
		private bool alone;
		
		public ClParameter(string name, string shortName,string description, bool unique, bool alone)
		{
			if (name==null)
				throw new ArgumentNullException("name");
			if (shortName==null)
				throw new ArgumentNullException("shortName");
			if (description==null)
				throw new ArgumentNullException("description");

			this.unique = unique;
			this.alone = alone;
			this.name = name;
			this.shortName = shortName;
			this.description = description;
		}
		
		public bool IsUnique
		{
			get
			{
				return this.unique || this.alone;
			}
		}
		
		public bool IsAlone
		{
			get
			{
				return  this.alone;
			}
		}
		
		public string Name
		{
			get
			{
				return this.name;
			}
		}
		
		public string ShortName
		{
			get
			{
				return this.shortName;
			}
		}
		
		public string Description
		{
			get
			{
				return this.description;
			}
		}
		
		public string ToHelpString()
		{
			return String.Format("--{0} -{1}, {2} {3} {4}",
				this.Name,
				this.ShortName,
				this.Description,
				this.IsUnique ?"unique": "multiple",
				this.IsAlone ? "alone": "with value"
				);
		}
	}
	
	/// <summary>
	/// Arguments class
	/// </summary>
	public class ClArguments
	{
		// Variables
		private ClParameterDictionary parameters;
		private StringDictionary uniqueParameters;
		private StringStringCollectionDictionary duplicateParameters;

		public ClArguments()
		{
			this.parameters = new ClParameterDictionary();	
			this.uniqueParameters = new StringDictionary();
			this.duplicateParameters = new StringStringCollectionDictionary();
			
			AddParameter(new ClParameter("help","h","Show this help",true,true));
		}
		
		public void AddParameter(ClParameter param)
		{
			this.parameters.Add(param);
		}
		
		public string ToHelpString()
		{
			StringWriter sw =  new StringWriter();
			
			sw.WriteLine("Command Line Parameter Help");
			sw.WriteLine("---------------------------");
			sw.WriteLine("Parameters can be called by setting -- with their full name,");
			sw.WriteLine("or - and their short name:");
			sw.WriteLine("   --help or -h");
			sw.WriteLine("Parameters can be followed by a value:");
			sw.WriteLine("   --input data");
			sw.WriteLine();
			sw.WriteLine("Name\tShortName\tDescription\tUnique\tAlone");
			foreach(DictionaryEntry de in this.parameters.Parameters)
			{
				ClParameter p = (ClParameter)de.Value;
				sw.WriteLine(p.ToHelpString());
			}
			
			return sw.ToString();
		}
		
		public void Parse(string[] Args)
		{
			try
			{
				Regex spliter=new Regex(
					@"^-{1,2}|^/|=|:",
					RegexOptions.IgnoreCase|RegexOptions.Compiled
					);
				Regex remover= new Regex(
					@"^['""]?(.*?)['""]?$",
					RegexOptions.IgnoreCase|RegexOptions.Compiled
					);
				string parameter=null;
				string[] parts;

				foreach(string Txt in Args)
				{
					// Look for new parameters (-,/ or --) and a possible enclosed value (=,:)
					parts=spliter.Split(Txt,3);
					switch(parts.Length)
					{
							// Found a value (for the last parameter found (space separator))
						case 1:
							if(parameter!=null)
							{
								if (CheckParameter(parameter))
								{
									parts[0]=remover.Replace(parts[0],"$1");								
									AddPair(parameter,parts[0]);
								}
								parameter=null;
							}
							// else Error: no parameter waiting for a value (skipped)
							break;
							// Found just a parameter
						case 2:
							// The last parameter is still waiting. With no value, set it to true.
							if(parameter!=null)
							{
								if (CheckParameter(parameter))
								{
									// add to unique, alone parameters
									AddUnique(parameter);								
								}
							}
							parameter=parts[1];
							break;
							// Parameter with enclosed value
						case 3:
							// The last parameter is still waiting. With no value, set it to true.
							if(parameter!=null)
							{
								AddUnique(parameter);
							}
							parameter=parts[1];
							// Remove possible enclosing characters (",')
							if(CheckParameter(parameter))
							{
								parts[2]=remover.Replace(parts[2],"$1");
								AddPair(parameter,parts[2]);
							}
							parameter=null;
							break;
					}
				}
				// In case a parameter is still waiting
				if(parameter!=null)
				{
					if(CheckParameter(parameter)) 
						AddUnique(parameter);
				}
			}
			catch(Exception)
			{
				Console.WriteLine(ToHelpString());
				throw;
			}
		}

		internal void AddPair(string key, string value)
		{
			ClParameter p = this.parameters[key];
			if (p.IsUnique)
			{
				if (this.uniqueParameters.ContainsKey(key))
					throw new Exception("Duplicate unique argument " + key);

				this.uniqueParameters.Add(p.Name,value.TrimEnd(' '));
				this.uniqueParameters.Add(p.ShortName,value.TrimEnd(' '));
			}
			else
			{
				if (!this.duplicateParameters.Contains(key))
				{
					this.duplicateParameters.Add(p.Name, new StringCollection());
					this.duplicateParameters.Add(p.ShortName, new StringCollection());
				}
			
				((StringCollection)this.duplicateParameters[p.Name]).Add(value.TrimEnd(' '));
				((StringCollection)this.duplicateParameters[p.ShortName]).Add(value.TrimEnd(' '));
			}
		}

		internal void AddUnique(string key)
		{
			if (this.uniqueParameters.ContainsKey(key))
				Console.WriteLine("Duplicate parameter: {0}", key);
			else
			{
				ClParameter p = this.parameters[key];
				this.uniqueParameters.Add(p.Name,"");
				this.uniqueParameters.Add(p.ShortName,"");
			}
		}
		
		internal bool CheckParameter(string key)
		{
			if (!this.parameters.Contains(key))
			{
				Console.WriteLine("Unknown parameter: {0}",key);
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
			return this.duplicateParameters.Contains(key);
		}

		public String Unique(string key)
		{
			return this.uniqueParameters[key];
		}	

		public StringCollection Duplicates(string key)
		{
			return (StringCollection)this.duplicateParameters[key];
		}

		public void ProcessHelp()
		{
			if (this.uniqueParameters.ContainsKey("h"))
				Console.WriteLine(ToHelpString());
		}
	}
}


using System;
using System.Collections;

namespace NPerf.Cons.Collections
{
	/// <summary>
	/// Summary description for ClParameterDictionary.
	/// </summary>
	public class ClParameterDictionary
	{
		private StringClParameterDictionary shortParameters;
		private StringClParameterDictionary parameters;

		public ClParameterDictionary()
		{
			this.shortParameters = new StringClParameterDictionary();
			this.parameters = new StringClParameterDictionary();
		}

		public void Add(ClParameter p)
		{
			if (p==null)
				throw new ArgumentNullException("p");

			this.shortParameters.Add(p.ShortName,p);
			this.parameters.Add(p.Name,p);
		}

		public bool Contains(string key)
		{
			if (key==null)
					throw new ArgumentNullException("key");
			if (this.shortParameters.Contains(key))
				return true;
			return this.parameters.Contains(key);
		
		}

		public ClParameter this[string key]
		{
			get
			{
				if (key==null)
					throw new ArgumentNullException("key");
				if (this.shortParameters.Contains(key))
					return this.shortParameters[key];
				return this.parameters[key];
			}
		}

		public IDictionary ShortParameters
		{
			get
			{
				return this.shortParameters;
			}
		}

		public IDictionary Parameters
		{
			get
			{
				return this.parameters;
			}
		}
	}
}

using System;
using System.Collections;

namespace NPerf.Core.Collections
{
	/// <summary>
	/// Summary description for AttributedMethodCollection.
	/// </summary>
	public class AttributedMethodCollection : IEnumerable
	{
		private Type testedType;
		private Type customAttributeType;

		public AttributedMethodCollection(Type testedType, Type customAttributeType)
		{
			if (testedType==null)
				throw new ArgumentNullException("testedType");
			if (customAttributeType==null)
				throw new ArgumentNullException("customAttributeType");
			this.testedType = testedType;
			this.customAttributeType = customAttributeType;
		}

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return new AttributedMethodEnumerator(
				this.testedType,
				this.customAttributeType
				);
		}

		#endregion
	}
}

using System;

namespace NPerf.Cons.Collections
{
	/// <summary>
	/// A dictionary with keys of type String and values of type ClParameter
	/// </summary>
	public class StringClParameterDictionary: System.Collections.DictionaryBase
	{
		/// <summary>
		/// Initializes a new empty instance of the StringClParameterDictionary class
		/// </summary>
		public StringClParameterDictionary()
		{
			// empty
		}

		/// <summary>
		/// Gets or sets the ClParameter associated with the given String
		/// </summary>
		/// <param name="key">
		/// The String whose value to get or set.
		/// </param>
		public virtual ClParameter this[String key]
		{
			get
			{
				return (ClParameter) this.Dictionary[key];
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this StringClParameterDictionary.
		/// </summary>
		/// <param name="key">
		/// The String key of the element to add.
		/// </param>
		/// <param name="value">
		/// The ClParameter value of the element to add.
		/// </param>
		public virtual void Add(String key, ClParameter value)
		{
			this.Dictionary.Add(key, value);
		}

		/// <summary>
		/// Determines whether this StringClParameterDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The String key to locate in this StringClParameterDictionary.
		/// </param>
		/// <returns>
		/// true if this StringClParameterDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool Contains(String key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this StringClParameterDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The String key to locate in this StringClParameterDictionary.
		/// </param>
		/// <returns>
		/// true if this StringClParameterDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsKey(String key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this StringClParameterDictionary contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The ClParameter value to locate in this StringClParameterDictionary.
		/// </param>
		/// <returns>
		/// true if this StringClParameterDictionary contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsValue(ClParameter value)
		{
			foreach (ClParameter item in this.Dictionary.Values)
			{
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this StringClParameterDictionary.
		/// </summary>
		/// <param name="key">
		/// The String key of the element to remove.
		/// </param>
		public virtual void Remove(String key)
		{
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this StringClParameterDictionary.
		/// </summary>
		public virtual System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this StringClParameterDictionary.
		/// </summary>
		public virtual System.Collections.ICollection Values
		{
			get
			{
				return this.Dictionary.Values;
			}
		}
	}
}

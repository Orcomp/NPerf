using System;
using System.Collections.Specialized;

namespace NPerf.Cons.Collections
{
	/// <summary>
	/// A dictionary with keys of type String and values of type StringCollection
	/// </summary>
	public class StringStringCollectionDictionary: System.Collections.DictionaryBase
	{
		/// <summary>
		/// Initializes a new empty instance of the StringStringCollectionDictionary class
		/// </summary>
		public StringStringCollectionDictionary()
		{
			// empty
		}

		/// <summary>
		/// Gets or sets the StringCollection associated with the given String
		/// </summary>
		/// <param name="key">
		/// The String whose value to get or set.
		/// </param>
		public virtual StringCollection this[String key]
		{
			get
			{
				return (StringCollection) this.Dictionary[key];
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this StringStringCollectionDictionary.
		/// </summary>
		/// <param name="key">
		/// The String key of the element to add.
		/// </param>
		/// <param name="value">
		/// The StringCollection value of the element to add.
		/// </param>
		public virtual void Add(String key, StringCollection value)
		{
			this.Dictionary.Add(key, value);
		}

		/// <summary>
		/// Determines whether this StringStringCollectionDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The String key to locate in this StringStringCollectionDictionary.
		/// </param>
		/// <returns>
		/// true if this StringStringCollectionDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool Contains(String key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this StringStringCollectionDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The String key to locate in this StringStringCollectionDictionary.
		/// </param>
		/// <returns>
		/// true if this StringStringCollectionDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsKey(String key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this StringStringCollectionDictionary contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The StringCollection value to locate in this StringStringCollectionDictionary.
		/// </param>
		/// <returns>
		/// true if this StringStringCollectionDictionary contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsValue(StringCollection value)
		{
			foreach (StringCollection item in this.Dictionary.Values)
			{
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this StringStringCollectionDictionary.
		/// </summary>
		/// <param name="key">
		/// The String key of the element to remove.
		/// </param>
		public virtual void Remove(String key)
		{
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this StringStringCollectionDictionary.
		/// </summary>
		public virtual System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this StringStringCollectionDictionary.
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

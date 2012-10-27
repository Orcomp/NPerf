using System;
using System.Reflection;

namespace NPerf.Core.Collections
{
	/// <summary>
	/// A collection of elements of type MethodInfo
	/// </summary>
	public class MethodInfoCollection: System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the MethodInfoCollection class.
		/// </summary>
		public MethodInfoCollection()
		{
			// empty
		}

		/// <summary>
		/// Initializes a new instance of the MethodInfoCollection class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new MethodInfoCollection.
		/// </param>
		public MethodInfoCollection(MethodInfo[] items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the MethodInfoCollection class, containing elements
		/// copied from another instance of MethodInfoCollection
		/// </summary>
		/// <param name="items">
		/// The MethodInfoCollection whose elements are to be added to the new MethodInfoCollection.
		/// </param>
		public MethodInfoCollection(MethodInfoCollection items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this MethodInfoCollection.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this MethodInfoCollection.
		/// </param>
		public virtual void AddRange(MethodInfo[] items)
		{
			foreach (MethodInfo item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another MethodInfoCollection to the end of this MethodInfoCollection.
		/// </summary>
		/// <param name="items">
		/// The MethodInfoCollection whose elements are to be added to the end of this MethodInfoCollection.
		/// </param>
		public virtual void AddRange(MethodInfoCollection items)
		{
			foreach (MethodInfo item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type MethodInfo to the end of this MethodInfoCollection.
		/// </summary>
		/// <param name="value">
		/// The MethodInfo to be added to the end of this MethodInfoCollection.
		/// </param>
		public virtual void Add(MethodInfo value)
		{
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic MethodInfo value is in this MethodInfoCollection.
		/// </summary>
		/// <param name="value">
		/// The MethodInfo value to locate in this MethodInfoCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this MethodInfoCollection;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(MethodInfo value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this MethodInfoCollection
		/// </summary>
		/// <param name="value">
		/// The MethodInfo value to locate in the MethodInfoCollection.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public virtual int IndexOf(MethodInfo value)
		{
			return this.List.IndexOf(value);
		}

		/// <summary>
		/// Inserts an element into the MethodInfoCollection at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the MethodInfo is to be inserted.
		/// </param>
		/// <param name="value">
		/// The MethodInfo to insert.
		/// </param>
		public virtual void Insert(int index, MethodInfo value)
		{
			this.List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the MethodInfo at the given index in this MethodInfoCollection.
		/// </summary>
		public virtual MethodInfo this[int index]
		{
			get
			{
				return (MethodInfo) this.List[index];
			}
			set
			{
				this.List[index] = value;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific MethodInfo from this MethodInfoCollection.
		/// </summary>
		/// <param name="value">
		/// The MethodInfo value to remove from this MethodInfoCollection.
		/// </param>
		public virtual void Remove(MethodInfo value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by MethodInfoCollection.GetEnumerator.
		/// </summary>
		public class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			public Enumerator(MethodInfoCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public MethodInfo Current
			{
				get
				{
					return (MethodInfo) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (MethodInfo) (this.wrapped.Current);
				}
			}

			public bool MoveNext()
			{
				return this.wrapped.MoveNext();
			}

			public void Reset()
			{
				this.wrapped.Reset();
			}
		}

		/// <summary>
		/// Returns an enumerator that can iterate through the elements of this MethodInfoCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new virtual MethodInfoCollection.Enumerator GetEnumerator()
		{
			return new MethodInfoCollection.Enumerator(this);
		}
	}

}

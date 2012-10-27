using System;

namespace NPerf.Core.Collections
{
	/// <summary>
	/// A collection of elements of type PerfTest
	/// </summary>
	public class PerfTestCollection: System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the PerfTestCollection class.
		/// </summary>
		public PerfTestCollection()
		{
			// empty
		}

		/// <summary>
		/// Initializes a new instance of the PerfTestCollection class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new PerfTestCollection.
		/// </param>
		public PerfTestCollection(PerfTest[] items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the PerfTestCollection class, containing elements
		/// copied from another instance of PerfTestCollection
		/// </summary>
		/// <param name="items">
		/// The PerfTestCollection whose elements are to be added to the new PerfTestCollection.
		/// </param>
		public PerfTestCollection(PerfTestCollection items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this PerfTestCollection.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this PerfTestCollection.
		/// </param>
		public virtual void AddRange(PerfTest[] items)
		{
			foreach (PerfTest item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another PerfTestCollection to the end of this PerfTestCollection.
		/// </summary>
		/// <param name="items">
		/// The PerfTestCollection whose elements are to be added to the end of this PerfTestCollection.
		/// </param>
		public virtual void AddRange(PerfTestCollection items)
		{
			foreach (PerfTest item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type PerfTest to the end of this PerfTestCollection.
		/// </summary>
		/// <param name="value">
		/// The PerfTest to be added to the end of this PerfTestCollection.
		/// </param>
		public virtual void Add(PerfTest value)
		{
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic PerfTest value is in this PerfTestCollection.
		/// </summary>
		/// <param name="value">
		/// The PerfTest value to locate in this PerfTestCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this PerfTestCollection;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(PerfTest value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this PerfTestCollection
		/// </summary>
		/// <param name="value">
		/// The PerfTest value to locate in the PerfTestCollection.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public virtual int IndexOf(PerfTest value)
		{
			return this.List.IndexOf(value);
		}

		/// <summary>
		/// Inserts an element into the PerfTestCollection at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the PerfTest is to be inserted.
		/// </param>
		/// <param name="value">
		/// The PerfTest to insert.
		/// </param>
		public virtual void Insert(int index, PerfTest value)
		{
			this.List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the PerfTest at the given index in this PerfTestCollection.
		/// </summary>
		public virtual PerfTest this[int index]
		{
			get
			{
				return (PerfTest) this.List[index];
			}
			set
			{
				this.List[index] = value;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific PerfTest from this PerfTestCollection.
		/// </summary>
		/// <param name="value">
		/// The PerfTest value to remove from this PerfTestCollection.
		/// </param>
		public virtual void Remove(PerfTest value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by PerfTestCollection.GetEnumerator.
		/// </summary>
		public class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			public Enumerator(PerfTestCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public PerfTest Current
			{
				get
				{
					return (PerfTest) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (PerfTest) (this.wrapped.Current);
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
		/// Returns an enumerator that can iterate through the elements of this PerfTestCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new virtual PerfTestCollection.Enumerator GetEnumerator()
		{
			return new PerfTestCollection.Enumerator(this);
		}
	}

}

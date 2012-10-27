using System;

namespace NPerf.Core.Collections
{
	/// <summary>
	/// A collection of elements of type PerfResult
	/// </summary>
	public class PerfResultCollection: System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the PerfResultCollection class.
		/// </summary>
		public PerfResultCollection()
		{
			// empty
		}

		/// <summary>
		/// Initializes a new instance of the PerfResultCollection class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new PerfResultCollection.
		/// </param>
		public PerfResultCollection(PerfResult[] items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the PerfResultCollection class, containing elements
		/// copied from another instance of PerfResultCollection
		/// </summary>
		/// <param name="items">
		/// The PerfResultCollection whose elements are to be added to the new PerfResultCollection.
		/// </param>
		public PerfResultCollection(PerfResultCollection items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this PerfResultCollection.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this PerfResultCollection.
		/// </param>
		public virtual void AddRange(PerfResult[] items)
		{
			foreach (PerfResult item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another PerfResultCollection to the end of this PerfResultCollection.
		/// </summary>
		/// <param name="items">
		/// The PerfResultCollection whose elements are to be added to the end of this PerfResultCollection.
		/// </param>
		public virtual void AddRange(PerfResultCollection items)
		{
			foreach (PerfResult item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type PerfResult to the end of this PerfResultCollection.
		/// </summary>
		/// <param name="value">
		/// The PerfResult to be added to the end of this PerfResultCollection.
		/// </param>
		public virtual void Add(PerfResult value)
		{
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic PerfResult value is in this PerfResultCollection.
		/// </summary>
		/// <param name="value">
		/// The PerfResult value to locate in this PerfResultCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this PerfResultCollection;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(PerfResult value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this PerfResultCollection
		/// </summary>
		/// <param name="value">
		/// The PerfResult value to locate in the PerfResultCollection.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public virtual int IndexOf(PerfResult value)
		{
			return this.List.IndexOf(value);
		}

		/// <summary>
		/// Inserts an element into the PerfResultCollection at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the PerfResult is to be inserted.
		/// </param>
		/// <param name="value">
		/// The PerfResult to insert.
		/// </param>
		public virtual void Insert(int index, PerfResult value)
		{
			this.List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the PerfResult at the given index in this PerfResultCollection.
		/// </summary>
		public virtual PerfResult this[int index]
		{
			get
			{
				return (PerfResult) this.List[index];
			}
			set
			{
				this.List[index] = value;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific PerfResult from this PerfResultCollection.
		/// </summary>
		/// <param name="value">
		/// The PerfResult value to remove from this PerfResultCollection.
		/// </param>
		public virtual void Remove(PerfResult value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by PerfResultCollection.GetEnumerator.
		/// </summary>
		public class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			public Enumerator(PerfResultCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public PerfResult Current
			{
				get
				{
					return (PerfResult) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (PerfResult) (this.wrapped.Current);
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
		/// Returns an enumerator that can iterate through the elements of this PerfResultCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new virtual PerfResultCollection.Enumerator GetEnumerator()
		{
			return new PerfResultCollection.Enumerator(this);
		}
	}
}

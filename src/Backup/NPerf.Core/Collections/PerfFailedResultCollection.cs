using System;

namespace NPerf.Core.Collections
{
	/// <summary>
	/// A collection of elements of type PerfFailedResult
	/// </summary>
	public class PerfFailedResultCollection: System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the PerfFailedResultCollection class.
		/// </summary>
		public PerfFailedResultCollection()
		{
			// empty
		}

		/// <summary>
		/// Initializes a new instance of the PerfFailedResultCollection class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new PerfFailedResultCollection.
		/// </param>
		public PerfFailedResultCollection(PerfFailedResult[] items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the PerfFailedResultCollection class, containing elements
		/// copied from another instance of PerfFailedResultCollection
		/// </summary>
		/// <param name="items">
		/// The PerfFailedResultCollection whose elements are to be added to the new PerfFailedResultCollection.
		/// </param>
		public PerfFailedResultCollection(PerfFailedResultCollection items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this PerfFailedResultCollection.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this PerfFailedResultCollection.
		/// </param>
		public virtual void AddRange(PerfFailedResult[] items)
		{
			foreach (PerfFailedResult item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another PerfFailedResultCollection to the end of this PerfFailedResultCollection.
		/// </summary>
		/// <param name="items">
		/// The PerfFailedResultCollection whose elements are to be added to the end of this PerfFailedResultCollection.
		/// </param>
		public virtual void AddRange(PerfFailedResultCollection items)
		{
			foreach (PerfFailedResult item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type PerfFailedResult to the end of this PerfFailedResultCollection.
		/// </summary>
		/// <param name="value">
		/// The PerfFailedResult to be added to the end of this PerfFailedResultCollection.
		/// </param>
		public virtual void Add(PerfFailedResult value)
		{
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic PerfFailedResult value is in this PerfFailedResultCollection.
		/// </summary>
		/// <param name="value">
		/// The PerfFailedResult value to locate in this PerfFailedResultCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this PerfFailedResultCollection;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(PerfFailedResult value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this PerfFailedResultCollection
		/// </summary>
		/// <param name="value">
		/// The PerfFailedResult value to locate in the PerfFailedResultCollection.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public virtual int IndexOf(PerfFailedResult value)
		{
			return this.List.IndexOf(value);
		}

		/// <summary>
		/// Inserts an element into the PerfFailedResultCollection at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the PerfFailedResult is to be inserted.
		/// </param>
		/// <param name="value">
		/// The PerfFailedResult to insert.
		/// </param>
		public virtual void Insert(int index, PerfFailedResult value)
		{
			this.List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the PerfFailedResult at the given index in this PerfFailedResultCollection.
		/// </summary>
		public virtual PerfFailedResult this[int index]
		{
			get
			{
				return (PerfFailedResult) this.List[index];
			}
			set
			{
				this.List[index] = value;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific PerfFailedResult from this PerfFailedResultCollection.
		/// </summary>
		/// <param name="value">
		/// The PerfFailedResult value to remove from this PerfFailedResultCollection.
		/// </param>
		public virtual void Remove(PerfFailedResult value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by PerfFailedResultCollection.GetEnumerator.
		/// </summary>
		public class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			public Enumerator(PerfFailedResultCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public PerfFailedResult Current
			{
				get
				{
					return (PerfFailedResult) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (PerfFailedResult) (this.wrapped.Current);
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
		/// Returns an enumerator that can iterate through the elements of this PerfFailedResultCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new virtual PerfFailedResultCollection.Enumerator GetEnumerator()
		{
			return new PerfFailedResultCollection.Enumerator(this);
		}
	}
}

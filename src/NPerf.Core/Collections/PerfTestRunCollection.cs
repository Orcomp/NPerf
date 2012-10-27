using System;

namespace NPerf.Core.Collections
{
	/// <summary>
	/// A collection of elements of type PerfTestRun
	/// </summary>
	public class PerfTestRunCollection: System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the PerfTestRunCollection class.
		/// </summary>
		public PerfTestRunCollection()
		{
			// empty
		}

		/// <summary>
		/// Initializes a new instance of the PerfTestRunCollection class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new PerfTestRunCollection.
		/// </param>
		public PerfTestRunCollection(PerfTestRun[] items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the PerfTestRunCollection class, containing elements
		/// copied from another instance of PerfTestRunCollection
		/// </summary>
		/// <param name="items">
		/// The PerfTestRunCollection whose elements are to be added to the new PerfTestRunCollection.
		/// </param>
		public PerfTestRunCollection(PerfTestRunCollection items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this PerfTestRunCollection.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this PerfTestRunCollection.
		/// </param>
		public virtual void AddRange(PerfTestRun[] items)
		{
			foreach (PerfTestRun item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another PerfTestRunCollection to the end of this PerfTestRunCollection.
		/// </summary>
		/// <param name="items">
		/// The PerfTestRunCollection whose elements are to be added to the end of this PerfTestRunCollection.
		/// </param>
		public virtual void AddRange(PerfTestRunCollection items)
		{
			foreach (PerfTestRun item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type PerfTestRun to the end of this PerfTestRunCollection.
		/// </summary>
		/// <param name="value">
		/// The PerfTestRun to be added to the end of this PerfTestRunCollection.
		/// </param>
		public virtual void Add(PerfTestRun value)
		{
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic PerfTestRun value is in this PerfTestRunCollection.
		/// </summary>
		/// <param name="value">
		/// The PerfTestRun value to locate in this PerfTestRunCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this PerfTestRunCollection;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(PerfTestRun value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this PerfTestRunCollection
		/// </summary>
		/// <param name="value">
		/// The PerfTestRun value to locate in the PerfTestRunCollection.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public virtual int IndexOf(PerfTestRun value)
		{
			return this.List.IndexOf(value);
		}

		/// <summary>
		/// Inserts an element into the PerfTestRunCollection at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the PerfTestRun is to be inserted.
		/// </param>
		/// <param name="value">
		/// The PerfTestRun to insert.
		/// </param>
		public virtual void Insert(int index, PerfTestRun value)
		{
			this.List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the PerfTestRun at the given index in this PerfTestRunCollection.
		/// </summary>
		public virtual PerfTestRun this[int index]
		{
			get
			{
				return (PerfTestRun) this.List[index];
			}
			set
			{
				this.List[index] = value;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific PerfTestRun from this PerfTestRunCollection.
		/// </summary>
		/// <param name="value">
		/// The PerfTestRun value to remove from this PerfTestRunCollection.
		/// </param>
		public virtual void Remove(PerfTestRun value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by PerfTestRunCollection.GetEnumerator.
		/// </summary>
		public class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			public Enumerator(PerfTestRunCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public PerfTestRun Current
			{
				get
				{
					return (PerfTestRun) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (PerfTestRun) (this.wrapped.Current);
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
		/// Returns an enumerator that can iterate through the elements of this PerfTestRunCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new virtual PerfTestRunCollection.Enumerator GetEnumerator()
		{
			return new PerfTestRunCollection.Enumerator(this);
		}
	}

}

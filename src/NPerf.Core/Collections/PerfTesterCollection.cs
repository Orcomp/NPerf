using System;

namespace NPerf.Core.Collections
{
	/// <summary>
	/// A collection of elements of type PerfTester
	/// </summary>
	public class PerfTesterCollection: System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the PerfTesterCollection class.
		/// </summary>
		public PerfTesterCollection()
		{
			// empty
		}

		/// <summary>
		/// Initializes a new instance of the PerfTesterCollection class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new PerfTesterCollection.
		/// </param>
		public PerfTesterCollection(PerfTester[] items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the PerfTesterCollection class, containing elements
		/// copied from another instance of PerfTesterCollection
		/// </summary>
		/// <param name="items">
		/// The PerfTesterCollection whose elements are to be added to the new PerfTesterCollection.
		/// </param>
		public PerfTesterCollection(PerfTesterCollection items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this PerfTesterCollection.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this PerfTesterCollection.
		/// </param>
		public virtual void AddRange(PerfTester[] items)
		{
			foreach (PerfTester item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another PerfTesterCollection to the end of this PerfTesterCollection.
		/// </summary>
		/// <param name="items">
		/// The PerfTesterCollection whose elements are to be added to the end of this PerfTesterCollection.
		/// </param>
		public virtual void AddRange(PerfTesterCollection items)
		{
			foreach (PerfTester item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type PerfTester to the end of this PerfTesterCollection.
		/// </summary>
		/// <param name="value">
		/// The PerfTester to be added to the end of this PerfTesterCollection.
		/// </param>
		public virtual void Add(PerfTester value)
		{
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic PerfTester value is in this PerfTesterCollection.
		/// </summary>
		/// <param name="value">
		/// The PerfTester value to locate in this PerfTesterCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this PerfTesterCollection;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(PerfTester value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this PerfTesterCollection
		/// </summary>
		/// <param name="value">
		/// The PerfTester value to locate in the PerfTesterCollection.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public virtual int IndexOf(PerfTester value)
		{
			return this.List.IndexOf(value);
		}

		/// <summary>
		/// Inserts an element into the PerfTesterCollection at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the PerfTester is to be inserted.
		/// </param>
		/// <param name="value">
		/// The PerfTester to insert.
		/// </param>
		public virtual void Insert(int index, PerfTester value)
		{
			this.List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the PerfTester at the given index in this PerfTesterCollection.
		/// </summary>
		public virtual PerfTester this[int index]
		{
			get
			{
				return (PerfTester) this.List[index];
			}
			set
			{
				this.List[index] = value;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific PerfTester from this PerfTesterCollection.
		/// </summary>
		/// <param name="value">
		/// The PerfTester value to remove from this PerfTesterCollection.
		/// </param>
		public virtual void Remove(PerfTester value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by PerfTesterCollection.GetEnumerator.
		/// </summary>
		public class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			public Enumerator(PerfTesterCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public PerfTester Current
			{
				get
				{
					return (PerfTester) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (PerfTester) (this.wrapped.Current);
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
		/// Returns an enumerator that can iterate through the elements of this PerfTesterCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new virtual PerfTesterCollection.Enumerator GetEnumerator()
		{
			return new PerfTesterCollection.Enumerator(this);
		}
	}
}

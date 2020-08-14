using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StandardLibrary.Collections
{
	public abstract class ListBase<T> : IList<T>, IList, IReadOnlyList<T>
	{
		private readonly List<T> innerList;

		public ListBase() => innerList = new List<T>();
		public ListBase(int capacity) => innerList = new List<T>(capacity);
		public ListBase(IEnumerable<T> collection) => innerList = new List<T>(collection);

		public virtual T this[int index] { get => ((IList<T>)innerList)[index]; set => ((IList<T>)innerList)[index] = value; }
		object IList.this[int index] { get => ((IList)innerList)[index]; set => ((IList)innerList)[index] = value; }

		public virtual int Count => ((ICollection<T>)innerList).Count;
		public virtual int Capacity => innerList.Capacity;
		public virtual bool IsReadOnly => ((ICollection<T>)innerList).IsReadOnly;
		public virtual bool IsFixedSize => ((IList)innerList).IsFixedSize;
		public virtual bool IsSynchronized => ((ICollection)innerList).IsSynchronized;
		public virtual object SyncRoot => ((ICollection)innerList).SyncRoot;


		public virtual void Add(T item) => ((ICollection<T>)innerList).Add(item);
		public virtual int Add(object value) => ((IList)innerList).Add(value);
		public virtual void AddRange(IEnumerable<T> collection) => innerList.AddRange(collection);
		public virtual ReadOnlyCollection<T> AsReadOnly() => innerList.AsReadOnly();
		public virtual int BinarySearch(T item) => innerList.BinarySearch(item);
		public virtual int BinarySearch(T item, IComparer<T> comparer) => innerList.BinarySearch(item, comparer);
		public virtual int BinarySearch(int index, int count, T item, IComparer<T> comparer) => innerList.BinarySearch(index, count, item, comparer);
		public virtual void Clear() => ((ICollection<T>)innerList).Clear();
		public virtual bool Contains(T item) => ((ICollection<T>)innerList).Contains(item);
		public virtual bool Contains(object value) => ((IList)innerList).Contains(value);
		public virtual List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter) => innerList.ConvertAll(converter);
		public virtual void CopyTo(int index, T[] array, int arrayIndex, int count) => innerList.CopyTo(index, array, arrayIndex, count);
		public virtual void CopyTo(T[] array, int arrayIndex) => innerList.CopyTo(array, arrayIndex);
		public virtual void CopyTo(T[] array) => innerList.CopyTo(array);
		public virtual void CopyTo(Array array, int index) => ((ICollection)innerList).CopyTo(array, index);
		public virtual T Find(Predicate<T> match) => innerList.Find(match);
		public virtual List<T> FindAll(Predicate<T> match) => innerList.FindAll(match);
		public virtual int FindIndex(int startIndex, int count, Predicate<T> match) => innerList.FindIndex(startIndex, count, match);
		public virtual int FindIndex(int startIndex, Predicate<T> match) => innerList.FindIndex(startIndex, match);
		public virtual int FindIndex(Predicate<T> match) => innerList.FindIndex(match);
		public virtual void ForEach(Action<T> action) => innerList.ForEach(action);
		public virtual Enumerator GetEnumerator() => (Enumerator)innerList.GetEnumerator();
		public virtual List<T> GetRange(int index, int count) => innerList.GetRange(index, count);
		public virtual int IndexOf(T item, int index, int count) => innerList.IndexOf(item, index, count);
		public virtual int IndexOf(T item, int index) => innerList.IndexOf(item, index);
		public virtual int IndexOf(T item) => innerList.IndexOf(item);
		public virtual int IndexOf(object value) => ((IList)innerList).IndexOf(value);
		public virtual void Insert(int index, T item) => innerList.Insert(index, item);
		public virtual void Insert(int index, object value) => ((IList)innerList).Insert(index, value);
		public virtual void InsertRange(int index, IEnumerable<T> collection) => innerList.InsertRange(index, collection);
		public virtual int LastIndexOf(T item) => innerList.LastIndexOf(item);
		public virtual int LastIndexOf(T item, int index) => innerList.LastIndexOf(item, index);
		public virtual int LastIndexOf(T item, int index, int count) => innerList.LastIndexOf(item, index, count);
		public virtual bool Remove(T item) => innerList.Remove(item);
		public virtual void Remove(object value) => ((IList)innerList).Remove(value);
		public virtual int RemoveAll(Predicate<T> match) => innerList.RemoveAll(match);
		public virtual void RemoveAt(int index) => innerList.RemoveAt(index);
		public virtual void RemoveRange(int index, int count) => innerList.RemoveRange(index, count);
		public virtual void Reverse(int index, int count) => innerList.Reverse(index, count);
		public virtual void Reverse() => innerList.Reverse();
		public virtual void Sort(Comparison<T> comparison) => innerList.Sort(comparison);
		public virtual void Sort(int index, int count, IComparer<T> comparer) => innerList.Sort(index, count, comparer);
		public virtual void Sort() => innerList.Sort();
		public virtual void Sort(IComparer<T> comparer) => innerList.Sort(comparer);
		public virtual T[] ToArray() => innerList.ToArray();
		public virtual void TrimExcess() => innerList.TrimExcess();
		public virtual bool TrueForAll(Predicate<T> match) => innerList.TrueForAll(match);
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)innerList).GetEnumerator();
		IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)innerList).GetEnumerator();

		public class Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			private List<T>.Enumerator enumeratorBase;

			public Enumerator(ListBase<T> list)
			{
				enumeratorBase = list.innerList.GetEnumerator();
			}

			private Enumerator(List<T>.Enumerator enumeratorBase)
			{
				this.enumeratorBase = enumeratorBase;
			}

			public virtual T Current => enumeratorBase.Current;

			object IEnumerator.Current => ((IEnumerator)enumeratorBase).Current;
			public virtual void Dispose() => enumeratorBase.Dispose();
			public virtual bool MoveNext() => enumeratorBase.MoveNext();
			public virtual void Reset() => ((IEnumerator)enumeratorBase).Reset();

			
			public static implicit operator Enumerator(List<T>.Enumerator enumerator)
			{
				return new Enumerator(enumerator);
			}

			public static implicit operator List<T>.Enumerator(Enumerator enumerator)
			{
				return enumerator.enumeratorBase;
			}
		}


		public static explicit operator List<T>(ListBase<T> ts)
		{
			if (ts is ListBaseRealisationForListCasting) throw new InvalidCastException("Can't cast object to List<T>");
			else return ts.innerList;
		}

		public static implicit operator ListBase<T>(List<T> l)
		{
			return new ListBaseRealisationForListCasting(l);
		}


		private sealed class ListBaseRealisationForListCasting : ListBase<T>
		{
			public ListBaseRealisationForListCasting() => _ = 0;
			public ListBaseRealisationForListCasting(int capacity) : base(capacity) => _ = 0;
			public ListBaseRealisationForListCasting(IEnumerable<T> collection) : base(collection) => _ = 0;
		}
	}
}

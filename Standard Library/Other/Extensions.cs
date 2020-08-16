using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Other
{
	public static class Extensions
	{
		public static void InvokeForAll<T>(this IEnumerable<T> obj, Action<T> action)
		{
			foreach (var item in obj)
			{
				action.Invoke(item);
			}
		}


		public static long IndexOfLong<T>(this IEnumerable<T> obj, T item) => IndexOfLong(obj, item, EqualityComparer<T>.Default);

		public static long IndexOf<T>(this IEnumerable<T> obj, T item) => IndexOfLong(obj, item, EqualityComparer<T>.Default);
		
		public static long IndexOf<T>(this IEnumerable<T> obj, T item, IEqualityComparer<T> comparer) => IndexOfLong(obj, item, comparer);

		public static long IndexOfLong<T>(this IEnumerable<T> obj, T item, IEqualityComparer<T> comparer)
		{
			using (var e = obj.GetEnumerator())
			{
				long i = 0;

				while (e.MoveNext())
				{
					if (comparer.Equals(e.Current, item))
					{
						return i;
					}

					i++;
				}

				//i--;
			}

			return -1;
		}
	}
}

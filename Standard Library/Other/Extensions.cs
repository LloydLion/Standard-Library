using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Other
{
	public static class Extensions
	{
		/// <summary>
		/// 
		///		Invokes function for each element in collection
		/// 
		/// </summary>
		/// <typeparam name="T">Collection type</typeparam>
		/// <param name="obj">Collection</param>
		/// <param name="action">Function for invoke</param>
		public static void InvokeForAll<T>(this IEnumerable<T> obj, Action<T> action)
		{
			foreach (var item in obj)
			{
				action.Invoke(item);
			}
		}


		/// <summary>
		/// 
		///		Search element in collection and return his index
		///		*Long version
		/// 
		/// </summary>
		/// <typeparam name="T">Collection type</typeparam>
		/// <param name="obj">Collection</param>
		/// <param name="item">Element for search</param>
		/// <returns>Index of element</returns>
		public static long IndexOfLong<T>(this IEnumerable<T> obj, T item) => IndexOfLong(obj, item, EqualityComparer<T>.Default);

		/// <summary>
		/// 
		///		Search element in collection and return his index
		/// 
		/// </summary>
		/// <typeparam name="T">Collection type</typeparam>
		/// <param name="obj">Collection</param>
		/// <param name="item">Element for search</param>
		/// <returns>Index of element</returns>
		public static long IndexOf<T>(this IEnumerable<T> obj, T item) => IndexOfLong(obj, item, EqualityComparer<T>.Default);

		/// <summary>
		/// 
		///		Search element in collection and return his index
		///		*Long version
		/// 
		/// </summary>
		/// <typeparam name="T">Collection type</typeparam>
		/// <param name="obj">Collection</param>
		/// <param name="item">Element for search</param>
		/// <param name="comparer">Custom equality comparer for elements</param>
		/// <returns>Index of element</returns>
		public static long IndexOf<T>(this IEnumerable<T> obj, T item, IEqualityComparer<T> comparer) => IndexOfLong(obj, item, comparer);

		/// <summary>
		/// 
		///		Search element in collection and return his index
		///		*Long version
		/// 
		/// </summary>
		/// <typeparam name="T">Collection type</typeparam>
		/// <param name="obj">Collection</param>
		/// <param name="item">Element for search</param>
		/// <param name="comparer">Custom equality comparer for elements</param>
		/// <returns>Index of element</returns>
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

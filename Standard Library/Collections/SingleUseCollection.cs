using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Collections
{
	public class SingleUseCollection<T> : ListBase<T>
	{
		public override T this[int index]
		{
			get { RemoveAt(index); return base[index]; }
			set { base[index] = value; }
		}
	}
}

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

		public override Enumerator GetEnumerator()
		{
			return new MyEnumerator(this);
		}

		private class MyEnumerator : Enumerator
		{
			private readonly SingleUseCollection<T> ts;
			private int currentIndex;
			private readonly List<int> toRemove;

			public MyEnumerator(SingleUseCollection<T> ts) : base(ts)
			{
				this.ts = ts;
				toRemove = new List<int>();
			}

			public override bool MoveNext()
			{
				toRemove.Add(currentIndex);
				var tmp = base.MoveNext();
				if (tmp == true) currentIndex++;
				return tmp;
			}

			public override void Reset()
			{
				currentIndex = 0;
				base.Reset();
			}

			public override void Dispose()
			{
				foreach (var item in toRemove)
					ts.RemoveAt(item);
				
				base.Dispose();
			}
		}
	}
}

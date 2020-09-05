using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Other.Stade
{
	public class StadeSelectedEventArgs<TEnum, TObject> : EventArgs where TEnum : Enum where TObject : class
	{
		public TEnum LastStade { get; }

		public StadeSelectedEventArgs(TEnum lastStade)
		{
			LastStade = lastStade;
		}
	}
}

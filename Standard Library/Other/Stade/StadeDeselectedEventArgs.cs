using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Other.Stade
{
	public class StadeDeselectedEventArgs<TEnum, TObject> : EventArgs where TEnum : Enum where TObject : class
	{
		public TEnum NewStade { get; }

		public StadeDeselectedEventArgs(TEnum newStade)
		{
			NewStade = newStade;
		}
	}
}

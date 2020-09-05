using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Other.Stade
{
	public interface IStade<TEnum, TObject> where TEnum : Enum where TObject : class
	{
		TEnum ShortcutValue { get; }
		TObject ControledObject { set; }


		void OnDeselected(StadeDeselectedEventArgs<TEnum, TObject> args);
		void OnSelected(StadeSelectedEventArgs<TEnum, TObject> args);
	}
}

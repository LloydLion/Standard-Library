using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Other.Stade
{
	public abstract class StadeBase<TEnum, TObject> : IStade<TEnum, TObject> where TEnum : Enum where TObject : class
	{
		public abstract TEnum ShortcutValue { get; }
		public abstract TObject ControledObject { set; }

		public virtual void OnDeselected(StadeDeselectedEventArgs<TEnum, TObject> args) { }
		public virtual void OnSelected(StadeSelectedEventArgs<TEnum, TObject> args) { }
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Other.Stade
{
	public class StandardStade<TEnum, TOBject> : StadeBase<TEnum, TOBject> where TEnum : Enum where TOBject : class
	{
		private readonly Action<TOBject, StadeSelectedEventArgs<TEnum, TOBject>> onSelect;
		private readonly Action<TOBject, StadeDeselectedEventArgs<TEnum, TOBject>> onDeselect;
		private TOBject ctrl;

		public StandardStade(TEnum shortcut, Action<TOBject, StadeSelectedEventArgs<TEnum, TOBject>> onSelect)
		{
			ShortcutValue = shortcut;
			this.onSelect = onSelect;
		}

		public StandardStade(TEnum shortcut, Action<TOBject, StadeSelectedEventArgs<TEnum, TOBject>> onSelect, Action<TOBject, StadeDeselectedEventArgs<TEnum, TOBject>> onDeselect) : this(shortcut, onSelect)
		{
			this.onDeselect = onDeselect;
		}


		public override TEnum ShortcutValue { get; }
		public override TOBject ControledObject { set => ctrl = value; }


		public override void OnDeselected(StadeDeselectedEventArgs<TEnum, TOBject> args)
		{
			onDeselect.Invoke(ctrl, args);
		}

		public override void OnSelected(StadeSelectedEventArgs<TEnum, TOBject> args)
		{
			onSelect.Invoke(ctrl, args);
		}
	}
}

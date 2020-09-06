using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Other.Stade
{
	/// <summary>
	/// 
	///		Standard realistaion of StadeBase<> and IStade<> with dynamic event handlers
	/// 
	/// </summary>
	/// <typeparam name="TEnum">Type of enum used for Shortcut value</typeparam>
	/// <typeparam name="TObject">Type of controled object</typeparam>
	public class StandardStade<TEnum, TOBject> : StadeBase<TEnum, TOBject> where TEnum : Enum where TOBject : class
	{
		private readonly Action<TOBject, StadeSelectedEventArgs<TEnum, TOBject>> onSelect;
		private readonly Action<TOBject, StadeDeselectedEventArgs<TEnum, TOBject>> onDeselect;
		private TOBject ctrl;

		/// <summary>
		/// 
		///		Inits a new StandardStade<> with given event handlers
		/// 
		/// </summary>
		/// <param name="shortcut">Stade Shortcut</param>
		/// <param name="onSelect">OnSelected event handler</param>
		public StandardStade(TEnum shortcut, Action<TOBject, StadeSelectedEventArgs<TEnum, TOBject>> onSelect) : this(shortcut, onSelect, (a, s) => { }) { }

		/// <summary>
		/// 
		///		Inits a new StandardStade<> with given event handlers
		/// 
		/// </summary>
		/// <param name="shortcut">Stade Shortcut</param>
		/// <param name="onSelect">OnSelected event handler</param>
		/// <param name="onDeselect">OnDeselected event handler</param>
		public StandardStade(TEnum shortcut, Action<TOBject, StadeSelectedEventArgs<TEnum, TOBject>> onSelect, Action<TOBject, StadeDeselectedEventArgs<TEnum, TOBject>> onDeselect)
		{
			ShortcutValue = shortcut;
			this.onSelect = onSelect;
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

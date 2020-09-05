using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Other.Stade
{
	/// <summary>
	/// 
	///		Interface for all stades
	/// 
	/// </summary>
	/// <typeparam name="TEnum">Type of enum used for Shortcut value</typeparam>
	/// <typeparam name="TObject">Type of controled object</typeparam>
	public interface IStade<TEnum, TObject> where TEnum : Enum where TObject : class
	{
		/// <summary>
		/// 
		///		Shortcut value 
		///		*use for select stade in List
		/// 
		/// </summary>
		TEnum ShortcutValue { get; }
		/// <summary>
		/// 
		///		Controled object
		///		Sets by StadeMachine object
		/// 
		/// </summary>
		TObject ControledObject { set; }


		/// <summary>
		/// 
		///		Event Method
		///		Invokes by StadeMachine on stade deselecting
		/// 
		/// </summary>
		/// <param name="args">Event args</param>
		void OnDeselected(StadeDeselectedEventArgs<TEnum, TObject> args);

		/// <summary>
		/// 
		///		Event Method
		///		Invokes by StadeMachine on stade selecting
		/// 
		/// </summary>
		/// <param name="args">Event args</param>
		void OnSelected(StadeSelectedEventArgs<TEnum, TObject> args);
	}
}

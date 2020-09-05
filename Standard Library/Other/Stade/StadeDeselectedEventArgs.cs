using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Other.Stade
{
	/// <summary>
	/// 
	///		Event args for IStade<>.OnDeselected event method
	/// 
	/// </summary>
	/// <typeparam name="TEnum">Type of enum used for Shortcut value</typeparam>
	/// <typeparam name="TObject">Type of controled object</typeparam>
	public class StadeDeselectedEventArgs<TEnum, TObject> : EventArgs where TEnum : Enum where TObject : class
	{
		/// <summary>
		/// 
		///		New selected stade
		/// 
		/// </summary>
		public TEnum NewStade { get; }


		/// <summary>
		/// 
		///		Inits a new StadeDeselectedEventArgs<> object
		/// 
		/// </summary>
		/// <param name="newStade">NewStade property value</param>
		public StadeDeselectedEventArgs(TEnum newStade)
		{
			NewStade = newStade;
		}
	}
}

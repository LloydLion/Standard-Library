using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Other.Stade
{
	/// <summary>
	/// 
	///		Event args for IStade<>.OnSelected event method
	/// 
	/// </summary>
	/// <typeparam name="TEnum">Type of enum used for Shortcut value</typeparam>
	/// <typeparam name="TObject">Type of controled object</typeparam>
	public class StadeSelectedEventArgs<TEnum, TObject> : EventArgs where TEnum : Enum where TObject : class
	{
		/// <summary>
		/// 
		///		Last selected stade
		/// 
		/// </summary>
		public TEnum LastStade { get; }

		/// <summary>
		/// 
		///		Inits a new StadeSelectedEventArgs<> object
		/// 
		/// </summary>
		/// <param name="newStade">LastStade property value</param>
		public StadeSelectedEventArgs(TEnum lastStade)
		{
			LastStade = lastStade;
		}
	}
}

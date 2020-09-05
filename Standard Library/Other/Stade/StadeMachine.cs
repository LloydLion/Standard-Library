using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StandardLibrary.Other.Stade
{
	/// <summary>
	/// 
	///		Smart application stade switcher
	/// 
	/// </summary>
	/// <typeparam name="TEnum">Type of enum used for Shortcut value</typeparam>
	/// <typeparam name="TObject">Type of controled object</typeparam>
	/// <typeparam name="TStade">Type of using Stade objects</typeparam>
	public class StadeMachine<TEnum, TObject, TStade> where TEnum : Enum where TObject : class where TStade : IStade<TEnum, TObject>
	{
		private TStade currentStade;


		/// <summary>
		/// 
		///		Creates a new StadeMachine<> object and init his with target object and list of stades
		/// 
		/// </summary>
		/// <param name="stades">List of stades</param>
		/// <param name="control">Target object</param>
		public StadeMachine(TStade[] stades, TObject control)
		{
			ControledObject = control;
			AvailableStades = stades;
		}


		/// <summary>
		/// 
		///		Returns selected in stade machine stade
		/// 
		/// </summary>
		public TStade CurrentStade 
		{ 
			get => currentStade; 
			private set 
			{ 
				if(currentStade.ShortcutValue.Equals(value.ShortcutValue))
				{
					var t = currentStade;
					currentStade.OnDeselected(new StadeDeselectedEventArgs<TEnum, TObject>(value.ShortcutValue));
					currentStade = value;
					value.OnSelected(new StadeSelectedEventArgs<TEnum, TObject>(t.ShortcutValue));
				} 
			} 
		}

		/// <summary>
		/// 
		///		Returns given on constructing target object
		/// 
		/// </summary>
		public TObject ControledObject { get; private set; }
		/// <summary>
		/// 
		///		Returns given on constructing stades
		/// 
		/// </summary>
		public TStade[] AvailableStades { get; private set; }


		/// <summary>
		/// 
		///		Sets stade in stade machine, Invokes OnSelected and OnDeselected event in new stade and last stade respectively
		/// 
		/// </summary>
		/// <param name="stade">Stade shortcut</param>
		public void SetStade(TEnum stade)
		{
			CurrentStade = AvailableStades.Single((s) => s.ShortcutValue.Equals(stade));
		}

		/// <summary>
		/// 
		///		Search a stade from AvailableStades by shortcut
		/// 
		/// </summary>
		/// <param name="stade">Shortcut</param>
		/// <returns>Search result</returns>
		public TStade GetStade(TEnum stade)
		{
			return AvailableStades.Single((s) => s.ShortcutValue.Equals(stade));
		}
	}

	/// <summary>
	/// 
	///		Smart application stade switcher
	///		*Without Stade generic type
	/// 
	/// </summary>
	/// <typeparam name="TEnum">Type of enum used for Shortcut value</typeparam>
	/// <typeparam name="TObject">Type of controled object</typeparam>
	public class StadeMachine<TEnum, TObject> : StadeMachine<TEnum, TObject, IStade<TEnum, TObject>> where TEnum : Enum where TObject : class
	{
		/// <summary>
		/// 
		///		Creates a new StadeMachine<> object and init his with target object and list of stades
		/// 
		/// </summary>
		/// <param name="stades"></param>
		/// <param name="control"></param>
		public StadeMachine(IStade<TEnum, TObject>[] stades, TObject control) : base(stades, control) { }
	}
}

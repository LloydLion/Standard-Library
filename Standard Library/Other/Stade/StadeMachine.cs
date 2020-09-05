using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StandardLibrary.Other.Stade
{
	public class StadeMachine<TEnum, TObject, TStade> where TEnum : Enum where TObject : class where TStade : IStade<TEnum, TObject>
	{
		private TStade currentStade;


		public StadeMachine(TStade[] stades, TObject control)
		{
			ControledObject = control;
			AvailableStades = stades;
		}


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

		public TObject ControledObject { get; private set; }
		public TStade[] AvailableStades { get; private set; }


		public void SetStade(TEnum stade)
		{
			CurrentStade = AvailableStades.Single((s) => s.ShortcutValue.Equals(stade));
		}

		public TStade GetStade(TEnum stade)
		{
			return AvailableStades.Single((s) => s.ShortcutValue.Equals(stade));
		}
	}

	public class StadeMachine<TEnum, TObject> : StadeMachine<TEnum, TObject, IStade<TEnum, TObject>> where TEnum : Enum where TObject : class
	{
		public StadeMachine(IStade<TEnum, TObject>[] stades, TObject control) : base(stades, control) { }
	}
}

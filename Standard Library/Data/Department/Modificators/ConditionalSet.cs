using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Data.Department.Modificators
{
	public class ConditionalSet : DepartmentPropertyModificator
	{
		private readonly Predicate<object> predicate;


		public override Type[] TargetTypes => null;

		
		public ConditionalSet(Predicate<object> predicate)
		{
			this.predicate = predicate;
		}


		public override OnSetOutputModel OnSet(OnSetInputModel input)
		{
			if(predicate.Invoke(input.DepartmentStore.GetPropertyValue(OnSetInputModel.PropertySettableValueProperty)) == false)
			{
				var model = base.OnSet(input);
				model.DepartmentStore.SetPropertyValue(OnSetOutputModel.IsChangePropertyValueProperty, true);
				model.DepartmentStore.SetPropertyValue(OnSetOutputModel.NewPropertyValueProperty,
					input.DepartmentStore.GetPropertyValue(OnSetInputModel.ValueStoreProperty).GetPropertyValue(input.DepartmentStore.GetPropertyValue(OnSetInputModel.PropertyInfoProperty)));

				return model;
			}
			else return base.OnSet(input);
		}
	}
}

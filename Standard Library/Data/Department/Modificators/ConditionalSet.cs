using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Data.Department.Modificators
{
	public class ConditionalSet<T> : DepartmentPropertyModificator
	{
		private readonly Predicate<T> predicate;


		public override Type[] TargetTypes => null;

		
		public ConditionalSet(Predicate<T> predicate)
		{
			this.predicate = predicate;
		}


		public override OnSetOutputModel OnSet(OnSetInputModel input)
		{
			if(predicate.Invoke((T)input.DepartmentStore.GetPropertyValue(OnSetInputModel.PropertyCurrentValueProperty)) == true)
			{
				var model = new OnSetOutputModel();
				model.DepartmentStore.SetPropertyValue(OnSetOutputModel.IsChangePropertyValueProperty, true);
				model.DepartmentStore.SetPropertyValue(OnSetOutputModel.NewPropertyValueProperty,
					input.DepartmentStore.GetPropertyValue(OnSetInputModel.ValueStoreProperty).GetPropertyValue(input.DepartmentStore.GetPropertyValue(OnSetInputModel.PropertyInfoProperty)));

				return model;
			}
			else return base.OnSet(input);
		}
	}
}

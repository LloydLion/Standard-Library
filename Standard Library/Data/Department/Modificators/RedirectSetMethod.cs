using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Data.Department.Modificators
{
	public class RedirectSetMethod : DepartmentPropertyModificator
	{
		private readonly Action<object> redirectMethod;

		public override Type[] TargetTypes => null;


		public RedirectSetMethod(Action<object> redirectMethod)
		{
			this.redirectMethod = redirectMethod;
		}


		public override OnSetOutputModel OnSet(OnSetInputModel input)
		{
			redirectMethod.Invoke(input.DepartmentStore.GetPropertyValue(OnSetInputModel.PropertySettableValueProperty));

			var model = base.OnSet(input);
			model.DepartmentStore.SetPropertyValue(OnSetOutputModel.IsChangePropertyValueProperty, true);
			model.DepartmentStore.SetPropertyValue(OnSetOutputModel.NewPropertyValueProperty,
				input.DepartmentStore.GetPropertyValue(OnSetInputModel.ValueStoreProperty).GetPropertyValue(input.DepartmentStore.GetPropertyValue(OnSetInputModel.PropertyInfoProperty)));
			return model;
		}
	}
}

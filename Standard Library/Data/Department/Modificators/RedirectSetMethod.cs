using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Data.Department.Modificators
{
	public class RedirectSetMethod<T> : DepartmentPropertyModificator
	{
		private readonly Action<T> redirectMethod;

		public override Type[] TargetTypes => null;


		public RedirectSetMethod(Action<T> redirectMethod)
		{
			this.redirectMethod = redirectMethod;
		}


		public override OnSetOutputModel OnSet(OnSetInputModel input)
		{
			redirectMethod.Invoke((T)input.DepartmentStore.GetPropertyValue(OnSetInputModel.PropertySettableValueProperty));

			var model = new OnSetOutputModel();
			model.DepartmentStore.SetPropertyValue(OnSetOutputModel.IsChangePropertyValueProperty, true);
			model.DepartmentStore.SetPropertyValue(OnSetOutputModel.NewPropertyValueProperty,
				input.DepartmentStore.GetPropertyValue(OnSetInputModel.ValueStoreProperty).GetPropertyValue(input.DepartmentStore.GetPropertyValue(OnSetInputModel.PropertyInfoProperty)));
			return model;
		}
	}
}

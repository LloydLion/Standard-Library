using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Data.Department.Modificators
{
	public class RedirectGetMethod<T> : DepartmentPropertyModificator
	{
		private readonly Func<T> redirectMethod;

		public override Type[] TargetTypes => null;


		public RedirectGetMethod(Func<T> redirectMethod)
		{
			this.redirectMethod = redirectMethod;
		}


		public override OnGetOutputModel OnGet(OnGetInputModel input)
		{
			var ret = base.OnGet(input);
			ret.DepartmentStore.SetPropertyValue(OnGetOutputModel.IsChangeReturnPropertyValueProperty, true);
			ret.DepartmentStore.SetPropertyValue(OnGetOutputModel.NewReturnPropertyValueProperty, redirectMethod.Invoke());

			return ret;
		}
	}
}

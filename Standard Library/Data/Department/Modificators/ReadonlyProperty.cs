using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Data.Department.Modificators
{
	public class ReadonlyProperty : DepartmentPropertyModificator
	{
		public override Type[] TargetTypes => null;


		public override OnSetOutputModel OnSet(OnSetInputModel input)
		{
			if (input.DepartmentStore.GetPropertyValue(OnSetInputModel.HasValidTokenProperty) == false)
			{
				throw new MemberAccessException("You need to have the VALID DepartmentPropertiesValuesStoreControlToken to acsees to set method of this property");
			}
			else
			{
				return base.OnSet(input);
			}
		}
	}
}

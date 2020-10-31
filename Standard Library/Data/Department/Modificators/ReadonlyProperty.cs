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
			if(input.DepartmentStore.GetPropertyValue(OnSetInputModel.HasValidTokenProperty))
			{
				throw new MemberAccessException("You need to have the VALID DepartmentPropertiesValuesStoreControlToken to acsees to set method of this property.\r\n" +
					"You are can't get it if you are not owner of the LocalDepartmentPropertiesValuesStore");
			}

			return new OnSetOutputModel();
		}
	}
}

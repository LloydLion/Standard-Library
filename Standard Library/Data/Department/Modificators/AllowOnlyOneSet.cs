using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StandardLibrary.Data.Department.Modificators
{
	public class AllowOnlyOneSet : DepartmentPropertyModificator
	{
		private readonly ReadonlyProperty support = new ReadonlyProperty();
		private readonly List<LocalDepartmentPropertiesValuesStore> isGettedAcsees = new List<LocalDepartmentPropertiesValuesStore>();


		public override Type[] TargetTypes => null;


		public override OnSetOutputModel OnSet(OnSetInputModel input)
		{
			if(isGettedAcsees.Contains(input.DepartmentStore.GetPropertyValue(OnSetInputModel.ValueStoreProperty)))
				return support.OnSet(input);

			isGettedAcsees.Add(input.DepartmentStore.GetPropertyValue(OnSetInputModel.ValueStoreProperty));
			return base.OnSet(input);
		}
	}
}

using StandardLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Data.Department
{
	public sealed class DepartmentPropertiesValuesStoreControlToken
	{
		internal LocalDepartmentPropertiesValuesStore TargetStore { get; }


		internal DepartmentPropertiesValuesStoreControlToken(LocalDepartmentPropertiesValuesStore targetStore)
		{
			TargetStore = targetStore;
		}
	}
}

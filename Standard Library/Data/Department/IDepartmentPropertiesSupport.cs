using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Data.Department
{
	public interface IDepartmentPropertiesSupport
	{
		LocalDepartmentPropertiesValuesStore DepartmentStore { get; }
	}
}

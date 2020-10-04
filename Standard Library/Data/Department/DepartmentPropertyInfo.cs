using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Data.Department
{
	public class DepartmentPropertyInfo<TProperty> : DepartmentPropertyInfo where TProperty : class
	{
		public DepartmentPropertyInfo(string name) : base(name, typeof(TProperty)) {  }

		public DepartmentPropertyInfo(string name, Type target) : base(name, typeof(TProperty), target) {  }
	}


	public abstract class DepartmentPropertyInfo
	{
		public Type PropertyType { get; }
		public string Name { get; }
		public Type TargetType { get; }
		public bool IsReadonly { get; private set; }


		protected DepartmentPropertyInfo(string name, Type value)
		{
			Name = name;
			PropertyType = value;
			TargetType = typeof(object);
		}

		protected DepartmentPropertyInfo(string name, Type value, Type target) : this(name, value)
		{
			TargetType = target;
		}


		public DepartmentPropertyInfo AsReadonly()
		{
			IsReadonly = true;
			return this;
		}
	}
}

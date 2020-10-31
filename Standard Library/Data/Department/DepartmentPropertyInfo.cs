using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StandardLibrary.Data.Department
{
	public class DepartmentPropertyInfo<TProperty> : DepartmentPropertyInfo
	{
		public DepartmentPropertyInfo(string name) : base(name, typeof(TProperty)) {  }

		public DepartmentPropertyInfo(string name, Type target) : base(name, typeof(TProperty), target) {  }


		public new DepartmentPropertyInfo<TProperty> AddModificator(DepartmentPropertyModificator modificator)
		{
			return (DepartmentPropertyInfo<TProperty>)base.AddModificator(modificator);
		}
	}


	public abstract class DepartmentPropertyInfo
	{
		public Type PropertyType { get; }
		public string Name { get; }
		public Type TargetType { get; }
		public bool IsReadonly { get; private set; }


		private readonly List<DepartmentPropertyModificator> modificators = new List<DepartmentPropertyModificator>();


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

		public DepartmentPropertyModificator[] GetModificators()
		{
			return modificators.ToArray();
		}

		public DepartmentPropertyInfo AddModificator(DepartmentPropertyModificator modificator)
		{
			if (modificator.TargetTypes == null || modificator.TargetTypes.Contains(TargetType))
			{
				modificators.Add(modificator);
				return this;
			}

			throw new ArgumentException("Can't apply this modificator. It has ivalid TargetType");
		}
	}
}

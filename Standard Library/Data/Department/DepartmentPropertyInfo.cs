using StandardLibrary.TypeManagment.Security;
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


		public new DepartmentPropertyInfo<TProperty> AddModificator(DepartmentPropertyModificator modificator, AccessToken token = null)
		{
			return (DepartmentPropertyInfo<TProperty>)base.AddModificator(modificator, token);
		}
	}


	public abstract class DepartmentPropertyInfo
	{
		public Type PropertyType { get; }
		public string Name { get; }
		public Type TargetType { get; }
		public bool IsReadonly { get; private set; }


		private readonly List<DepartmentPropertyModificator> modificators = new List<DepartmentPropertyModificator>();
		private readonly AccessTokenValidator validator;


		protected DepartmentPropertyInfo(string name, Type value, AccessToken token = null)
		{
			Name = name;
			PropertyType = value;
			TargetType = typeof(object);
			if(token is null) validator = new AccessTokenValidator(token);
		}

		protected DepartmentPropertyInfo(string name, Type value, Type target, AccessToken token = null) : this(name, value, token)
		{
			TargetType = target;
		}

		public DepartmentPropertyModificator[] GetModificators()
		{
			return modificators.ToArray();
		}

		public DepartmentPropertyInfo AddModificator(DepartmentPropertyModificator modificator, AccessToken token = null)
		{
			if(!(validator?.IsValid(token) ?? true)) throw new MemberAccessException("Can't apply this modificator. Access token is invlaid. Access deined");

			if (modificator.TargetTypes == null || modificator.TargetTypes.Contains(TargetType))
			{
				modificators.Add(modificator);
				return this;
			}
			else throw new ArgumentException("Can't apply this modificator. It has ivalid TargetType");
		}
	}
}

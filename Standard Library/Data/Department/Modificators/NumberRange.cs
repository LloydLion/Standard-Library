using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace StandardLibrary.Data.Department.Modificators
{
	public class NumberRange : DepartmentPropertyModificator
	{
		private readonly decimal min, max;


		public NumberRange(decimal min, decimal max)
		{
			if (min > max) throw new ArgumentOutOfRangeException(nameof(min), min, "[min] bigger then [max]");

			this.min = min;
			this.max = max;
		}


		public override Type[] TargetTypes =>  new Type[]
		{ typeof(int), typeof(uint), typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(long),
			typeof(ulong), typeof(float), typeof(double), typeof(decimal) };


		public override OnSetOutputModel OnSet(OnSetInputModel input)
		{
			decimal current = ((IConvertible)input.DepartmentStore.GetPropertyValue(OnSetInputModel.PropertySettableValueProperty) ?? 0).ToDecimal(new NumberFormatInfo());
			decimal newVal = Math.Max(Math.Min(current, max), min);

			OnSetOutputModel model = base.OnSet(input);

			model.DepartmentStore.SetPropertyValue(OnSetOutputModel.IsChangePropertyValueProperty, true);
			model.DepartmentStore.SetPropertyValue(OnSetOutputModel.NewPropertyValueProperty,
				((IConvertible)newVal).ToType(input.DepartmentStore.GetPropertyValue(OnSetInputModel.PropertyInfoProperty).PropertyType, new NumberFormatInfo()));


			return model;
		}
	}
}

using StandardLibrary.TypeManagment;
using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Data.Department
{
	public abstract class DepartmentPropertyModificator
	{
		public abstract Type[] TargetTypes { get; }


		public virtual OnGetOutputModel OnGet(OnGetInputModel input)
		{
			return new OnGetOutputModel();
		}

		public virtual OnSetOutputModel OnSet(OnSetInputModel input)
		{
			return new OnSetOutputModel();
		}

		public virtual OnEventOutputModel OnEvent(OnEventInputModel input)
		{
			return new OnEventOutputModel();
		}


		public class OnGetInputModel : IDepartmentPropertiesSupport
		{
			public static readonly DepartmentPropertyInfo<object> PropertyValueProperty = new DepartmentPropertyInfo<object>("PropertyValue");
			public static readonly DepartmentPropertyInfo<DepartmentPropertyInfo> PropertyInfoProperty = new DepartmentPropertyInfo<DepartmentPropertyInfo>("PropertyInfo");
			public static readonly DepartmentPropertyInfo<LocalDepartmentPropertiesValuesStore> ValueStoreProperty = new DepartmentPropertyInfo<LocalDepartmentPropertiesValuesStore>("ValueStoreProperty");


			public LocalDepartmentPropertiesValuesStore DepartmentStore { get; } = new LocalDepartmentPropertiesValuesStore()
				.PutProperty(PropertyValueProperty, PropertyInfoProperty, ValueStoreProperty);
		}

		public class OnGetOutputModel : IDepartmentPropertiesSupport
		{
			public static readonly DepartmentPropertyInfo<object> NewReturnPropertyValueProperty = new DepartmentPropertyInfo<object>("NewPropertyValue");
			public static readonly DepartmentPropertyInfo<object> NewRealPropertyValueProperty = new DepartmentPropertyInfo<object>("NewPropertyValue");
			public static readonly DepartmentPropertyInfo<bool> IsChangeReturnPropertyValueProperty = new DepartmentPropertyInfo<bool>("IsChangePropertyValue");
			public static readonly DepartmentPropertyInfo<bool> IsChangeRealPropertyValueProperty = new DepartmentPropertyInfo<bool>("IsChangePropertyValue");


			public LocalDepartmentPropertiesValuesStore DepartmentStore { get; } = new LocalDepartmentPropertiesValuesStore()
				.PutProperty(NewReturnPropertyValueProperty, NewRealPropertyValueProperty, IsChangeReturnPropertyValueProperty, IsChangeRealPropertyValueProperty);
		}


		public class OnSetInputModel : IDepartmentPropertiesSupport
		{
			public static readonly DepartmentPropertyInfo<object> PropertySettableValueProperty = new DepartmentPropertyInfo<object>("PropertySettableValue");
			public static readonly DepartmentPropertyInfo<object> PropertyCurrentValueProperty = new DepartmentPropertyInfo<object>("PropertyCurrentValue");
			public static readonly DepartmentPropertyInfo<bool> HasValidTokenProperty = new DepartmentPropertyInfo<bool>("HasValidToken");
			public static readonly DepartmentPropertyInfo<DepartmentPropertyInfo> PropertyInfoProperty = new DepartmentPropertyInfo<DepartmentPropertyInfo>("PropertyInfo");
			public static readonly DepartmentPropertyInfo<LocalDepartmentPropertiesValuesStore> ValueStoreProperty = new DepartmentPropertyInfo<LocalDepartmentPropertiesValuesStore>("ValueStoreProperty");


			public LocalDepartmentPropertiesValuesStore DepartmentStore { get; } = new LocalDepartmentPropertiesValuesStore().PutProperty(PropertySettableValueProperty);
		}

		public class OnSetOutputModel : IDepartmentPropertiesSupport
		{
			public static readonly DepartmentPropertyInfo<object> NewPropertyValueProperty = new DepartmentPropertyInfo<object>("NewPropertyValueProperty");
			public static readonly DepartmentPropertyInfo<bool> IsChangePropertyValueProperty = new DepartmentPropertyInfo<bool>("IsChangePropertyValue");
			

			public LocalDepartmentPropertiesValuesStore DepartmentStore { get; } = new LocalDepartmentPropertiesValuesStore().PutProperty(NewPropertyValueProperty, IsChangePropertyValueProperty);
		}


		public class OnEventInputModel : IDepartmentPropertiesSupport
		{
			public static readonly DepartmentPropertyInfo<EventArgs> EventArgsProperty = new DepartmentPropertyInfo<EventArgs>("EventArgs");
			public static readonly DepartmentPropertyInfo<object> InvokerProperty = new DepartmentPropertyInfo<object>("Invoker");
			public static readonly DepartmentPropertyInfo<LocalDepartmentPropertiesValuesStore> ValueStoreProperty = new DepartmentPropertyInfo<LocalDepartmentPropertiesValuesStore>("ValueStore");
			public static readonly DepartmentPropertyInfo<DepartmentPropertyInfo> PropertyInfoProperty = new DepartmentPropertyInfo<DepartmentPropertyInfo>("PropertyInfo");
			public static readonly DepartmentPropertyInfo<string> EventNameProperty = new DepartmentPropertyInfo<string>("EventName");
			public static readonly DepartmentPropertyInfo<EventType> EventTypeProperty = new DepartmentPropertyInfo<EventType>("EventType");


			public LocalDepartmentPropertiesValuesStore DepartmentStore { get; } = new LocalDepartmentPropertiesValuesStore()
				.PutProperty(EventArgsProperty, InvokerProperty, ValueStoreProperty, EventNameProperty, EventTypeProperty, PropertyInfoProperty);


			public enum EventType
			{
				PropertyChanging,
				PropertyChanged,
				Other
			}
		}

		public class OnEventOutputModel : IDepartmentPropertiesSupport
		{
			public static readonly DepartmentPropertyInfo<bool> IsCancelEventInvokeProperty = new DepartmentPropertyInfo<bool>("IsCancelEventInvoke");


			public LocalDepartmentPropertiesValuesStore DepartmentStore { get; } = new LocalDepartmentPropertiesValuesStore().PutProperty(IsCancelEventInvokeProperty);
		}
	}
}

using StandardLibrary.Interfaces;
using StandardLibrary.Models;
using StandardLibrary.Other;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StandardLibrary.Data.Department
{
	public class LocalDepartmentPropertiesValuesStore : INotifyPropertyChanged, INotifyPropertyChanging
	{
		private readonly Dictionary<DepartmentPropertyInfo, PropertyStade> values;
		private bool tokenIsGiven;
		private readonly bool enableSecure;


		public event PropertyChangedEventHandler PropertyChanged;
		public event PropertyChangingEventHandler PropertyChanging;


		public LocalDepartmentPropertiesValuesStore(bool enableSecure = true)
		{
			values = new Dictionary<DepartmentPropertyInfo, PropertyStade>();
			this.enableSecure = enableSecure;
		}


		public LocalDepartmentPropertiesValuesStore PutProperty(DepartmentPropertyInfo prop)
		{
			values.Add(prop, new PropertyStade());
			return this;
		}

		public LocalDepartmentPropertiesValuesStore PutProperty(params DepartmentPropertyInfo[] props)
		{
			props.InvokeForAll((s) => PutProperty(s));
			return this;
		}

		public void SetPropertyValue<T>(DepartmentPropertyInfo<T> info, T obj, DepartmentPropertiesValuesStoreControlToken token)
		{
			SetPropertyValueDirect(info, ApplySetModificators(info, obj, !enableSecure || CheckToken(token)));
		}

		public void SetPropertyValue<T>(DepartmentPropertyInfo<T> info, T obj)
		{
			SetPropertyValueDirect(info, ApplySetModificators(info, obj, !enableSecure));
		}

		public T GetPropertyValue<T>(DepartmentPropertyInfo<T> info)
		{
			return ApplyGetModificators(info);
		}

		public DepartmentPropertiesValuesStoreControlToken GetToken()
		{
			if (tokenIsGiven == true) throw new MemberAccessException("Token already generated can't generate it again");

			tokenIsGiven = true;
			return new DepartmentPropertiesValuesStoreControlToken(this);
		}

		public LocalDepartmentPropertiesValuesStore LockToken()
		{
			tokenIsGiven = true;
			return this;
		}

		public bool CheckToken(DepartmentPropertiesValuesStoreControlToken token)
		{
			if (tokenIsGiven == false) throw new InvalidOperationException("Can't check given token. Target token hasn't generated");

			return token.TargetStore == this;
		}

		private T ApplyGetModificators<T>(DepartmentPropertyInfo<T> info)
		{
			T returnValue = GetPropertyValueDirect(info);
			var mods = info.GetModificators();

			foreach (var mod in mods)
			{
				var input = new DepartmentPropertyModificator.OnGetInputModel();
				input.DepartmentStore.SetPropertyValue(DepartmentPropertyModificator.OnGetInputModel.PropertyValueProperty, returnValue);
				input.DepartmentStore.SetPropertyValue(DepartmentPropertyModificator.OnGetInputModel.PropertyInfoProperty, info);
				input.DepartmentStore.SetPropertyValue(DepartmentPropertyModificator.OnGetInputModel.ValueStoreProperty, this);

				var output = mod.OnGet(input);

				if(output.DepartmentStore.GetPropertyValue(DepartmentPropertyModificator.OnGetOutputModel.IsChangeReturnPropertyValueProperty) == true)
					returnValue = (T)output.DepartmentStore.GetPropertyValue(DepartmentPropertyModificator.OnGetOutputModel.NewReturnPropertyValueProperty);

				if(output.DepartmentStore.GetPropertyValue(DepartmentPropertyModificator.OnGetOutputModel.IsChangeRealPropertyValueProperty) == true)
					SetPropertyValue(info, (T)output.DepartmentStore.GetPropertyValue(DepartmentPropertyModificator.OnGetOutputModel.NewRealPropertyValueProperty));
			}

			return returnValue;
		}

		private T ApplySetModificators<T>(DepartmentPropertyInfo<T> info, T value, bool hasValidToken)
		{
			T returnValue = value;
			var mods = info.GetModificators();

			foreach (var mod in mods)
			{
				var input = new DepartmentPropertyModificator.OnSetInputModel();
				input.DepartmentStore.SetPropertyValue(DepartmentPropertyModificator.OnSetInputModel.PropertyCurrentValueProperty, GetPropertyValue(info));
				input.DepartmentStore.SetPropertyValue(DepartmentPropertyModificator.OnSetInputModel.PropertySettableValueProperty, value);
				input.DepartmentStore.SetPropertyValue(DepartmentPropertyModificator.OnSetInputModel.HasValidTokenProperty, hasValidToken);
				input.DepartmentStore.SetPropertyValue(DepartmentPropertyModificator.OnSetInputModel.PropertyInfoProperty, info);
				input.DepartmentStore.SetPropertyValue(DepartmentPropertyModificator.OnSetInputModel.ValueStoreProperty, this);

				var output = mod.OnSet(input);

				if(output.DepartmentStore.GetPropertyValue(DepartmentPropertyModificator.OnSetOutputModel.IsChangePropertyValueProperty))
					returnValue = (T)output.DepartmentStore.GetPropertyValue(DepartmentPropertyModificator.OnSetOutputModel.NewPropertyValueProperty);
			}

			return returnValue;
		}

		private T GetPropertyValueDirect<T>(DepartmentPropertyInfo<T> info) => (T)values[info].Value;

		private void SetPropertyValueDirect<T>(DepartmentPropertyInfo<T> info, T value)
		{
			OnPropertyChanging(info);
			values[info].Value = value;
			OnPropertyChanged(info);
		}

		private void OnPropertyChanging(DepartmentPropertyInfo info)
		{
			var args = new PropertyChangingEventArgs(info.Name);
			if(ApplyEventModificators(info, args, this, DepartmentPropertyModificator.OnEventInputModel.EventType.PropertyChanging) == true)
				PropertyChanging?.Invoke(this, args);
		}

		private void OnPropertyChanged(DepartmentPropertyInfo info)
		{
			var args = new PropertyChangedEventArgs(info.Name);
			if (ApplyEventModificators(info, args, this, DepartmentPropertyModificator.OnEventInputModel.EventType.PropertyChanging) == true)
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info.Name));
		}

		private bool ApplyEventModificators(DepartmentPropertyInfo info, EventArgs args, object invoker, DepartmentPropertyModificator.OnEventInputModel.EventType type, string eventName = null)
		{
			if(type != DepartmentPropertyModificator.OnEventInputModel.EventType.Other) eventName = type.ToString();
			else if(eventName != null) { }
			else throw new InvalidOperationException("Invalid eventName");

			var mods = info.GetModificators();
			bool result = false;

			foreach (var mod in mods)
			{
				var input = new DepartmentPropertyModificator.OnEventInputModel();
				input.DepartmentStore.SetPropertyValue(DepartmentPropertyModificator.OnEventInputModel.ValueStoreProperty, this);
				input.DepartmentStore.SetPropertyValue(DepartmentPropertyModificator.OnEventInputModel.PropertyInfoProperty, info);
				input.DepartmentStore.SetPropertyValue(DepartmentPropertyModificator.OnEventInputModel.InvokerProperty, invoker);
				input.DepartmentStore.SetPropertyValue(DepartmentPropertyModificator.OnEventInputModel.EventArgsProperty, args);
				input.DepartmentStore.SetPropertyValue(DepartmentPropertyModificator.OnEventInputModel.EventNameProperty, eventName);
				input.DepartmentStore.SetPropertyValue(DepartmentPropertyModificator.OnEventInputModel.EventTypeProperty, type);

				var output = mod.OnEvent(input);

				result = output.DepartmentStore.GetPropertyValue(DepartmentPropertyModificator.OnEventOutputModel.IsCancelEventInvokeProperty) == true || result;
			}

			return !result;
		}


		private class PropertyStade
		{
			public object Value { get; set; }
		}
	}


	public class ExtendedDepartmentPropertiesValuesStore : IFinalizeble
	{
		private readonly Dictionary<object, LocalDepartmentPropertiesValuesStore> store;
		private readonly List<DepartmentPropertyInfo> properties;


		public bool IsFinalized { get; private set; }


		public ExtendedDepartmentPropertiesValuesStore()
		{
			store = new Dictionary<object, LocalDepartmentPropertiesValuesStore>();
			properties = new List<DepartmentPropertyInfo>();
		}


		public void FinalizeObject()
		{
			IsFinalized = true;
		}

		public void PutProperty(DepartmentPropertyInfo prop)
		{
			if (IsFinalized) throw new InvalidOperationException("Object is finalized. You can't do any write action with this object");

			properties.Add(prop);
		}

		public void AddObject(object obj)
		{
			if (IsFinalized == false) throw new InvalidOperationException("Object is NOT finalized. You can't do this action before it");

			store.Add(obj, new LocalDepartmentPropertiesValuesStore(enableSecure: false));

			foreach (var item in properties)
			{
				if (item.TargetType.IsInstanceOfType(obj))
					GetObjectData(obj).PutProperty(item);
			}
		}

		public LocalDepartmentPropertiesValuesStore GetObjectData(object obj) => store[obj];
	}
}


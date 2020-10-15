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
		private bool enableSecure;


		public event PropertyChangedEventHandler PropertyChanged;
		public event PropertyChangingEventHandler PropertyChanging;


		public LocalDepartmentPropertiesValuesStore(bool enableSecure = true)
		{
			values = new Dictionary<DepartmentPropertyInfo, PropertyStade>();
			this.enableSecure = enableSecure;
		}


		public void PutProperty(DepartmentPropertyInfo prop)
		{
			values.Add(prop, new PropertyStade());
		}


		public void SetPropertyValue<T>(DepartmentPropertyInfo<T> info, T obj, DepartmentPropertiesValuesStoreControlToken token) where T : class
		{
			if(CheckToken(token) == false) throw new MemberAccessException("Given token is invalid");

			values[info].Value = obj;
		}

		public void SetPropertyValue<T>(DepartmentPropertyInfo<T> info, T obj) where T : class
		{
			if(info.IsReadonly == true && enableSecure == true) throw new MemberAccessException("Member is readonly. You need the VALID DepartmentPropertiesValuesStoreControlToken to get access");

			PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(info.Name));
			values[info].Value = obj;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info.Name));
		}

		public T GetPropertyValue<T>(DepartmentPropertyInfo<T> info) where T : class
		{
			return (T)values[info].Value;
		}

		public DepartmentPropertiesValuesStoreControlToken GetToken()
		{
			if (tokenIsGiven == true) throw new MemberAccessException("Token already generated can't generate it again");

			tokenIsGiven = true;
			return new DepartmentPropertiesValuesStoreControlToken(this);
		}

		public bool CheckToken(DepartmentPropertiesValuesStoreControlToken token)
		{
			if (tokenIsGiven == false) throw new InvalidOperationException("Can't check given token. Target token hasn't generated");

			return token.TargetStore == this;
		}


		internal class PropertyStade
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


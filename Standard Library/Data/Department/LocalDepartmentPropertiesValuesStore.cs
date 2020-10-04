using StandardLibrary.Interfaces;
using StandardLibrary.Models;
using StandardLibrary.Other;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StandardLibrary.Data.Department
{
	public class LocalDepartmentPropertiesValuesStore
	{
		private readonly List<DepartmentPropertyInfo> departmentProperties;
		private readonly Dictionary<DepartmentPropertyInfo, PropertyStade> values;
		private bool tokenIsGiven;


		public LocalDepartmentPropertiesValuesStore()
		{
			departmentProperties = new List<DepartmentPropertyInfo>();
			values = new Dictionary<DepartmentPropertyInfo, PropertyStade>();
		}


		public void PutProperty(DepartmentPropertyInfo prop)
		{
			departmentProperties.Add(prop);
		}


		public void SetPropertyValue<T>(DepartmentPropertyInfo<T> info, T obj, DepartmentPropertiesValuesStoreControlToken token) where T : class
		{
			if(CheckToken(token) == false) throw new MemberAccessException("Given token is invalid");

			values[info].Value = obj;
		}
		
		public void SetPropertyValue<T>(DepartmentPropertyInfo<T> info, T obj) where T : class
		{
			if(info.IsReadonly) throw new MemberAccessException("Member is readonly. You need the VALID DepartmentPropertiesValuesStoreControlToken to get access");

			values[info].Value = obj;
		}

		public T GetPropertyValue<T>(DepartmentPropertyInfo<T> info) where T : class
		{
			return (T)values[info].Value;
		}

		public PropertyEventsManager GetEventsManager(DepartmentPropertyInfo info)
		{
			return values[info].EventsManager;
		}

		public DepartmentPropertiesValuesStoreControlToken GetToken()
		{
			if(tokenIsGiven == true) throw new MemberAccessException("Token already generated can't generate it again");

			tokenIsGiven = true;
			return new DepartmentPropertiesValuesStoreControlToken(this);
		}

		private bool CheckToken(DepartmentPropertiesValuesStoreControlToken token)
		{
			if(tokenIsGiven == false) throw new InvalidOperationException("Can't check given token. Target token hasn't generated");

			return token.TargetStore == this;
		}


		internal class PropertyStade : INotifyPropertyChanged, INotifyPropertyChanging
		{
			private object value;


			public PropertyStade()
			{
				EventsManager = new PropertyEventsManager(this);
			}


			public object Value { get => value; set { PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Value))); this.value = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value))); } }
			public PropertyEventsManager EventsManager { get; }

			public event PropertyChangedEventHandler PropertyChanged;
			public event PropertyChangingEventHandler PropertyChanging;
		}

		public class PropertyEventsManager : INotifyPropertyChanging, INotifyPropertyChanged
		{
			public event PropertyChangingEventHandler PropertyChanging;
			public event PropertyChangedEventHandler PropertyChanged;


			internal PropertyEventsManager(PropertyStade stade)
			{
				stade.PropertyChanging += (s, q) => PropertyChanging?.Invoke(s, q);
				stade.PropertyChanged += (s, q) => PropertyChanged?.Invoke(s, q);
			}
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
			if(IsFinalized) throw new InvalidOperationException("Object is finalized. You can't do any write action with this object");

			properties.Add(prop);
		}

		public void AddObject(object obj)
		{
			if(IsFinalized == false) throw new InvalidOperationException("Object is NOT finalized. You can't do this action before it");

			store.Add(obj, new LocalDepartmentPropertiesValuesStore());

			foreach (var item in properties)
			{
				if (item.TargetType.IsInstanceOfType(obj))
					store[obj].PutProperty(item);
			}
		}

		public LocalDepartmentPropertiesValuesStore GetObjectData(object obj) => store[obj];
	}
}


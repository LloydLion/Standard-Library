﻿using Newtonsoft.Json;
using StandardLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace StandardLibrary.Models
{
	/// <summary>
	/// 
	///     Base class for all models    
	/// 
	/// </summary>
	[Serializable]
	public abstract class Model : IModel
	{
		[NonSerialized] public const string NotSupportedExceptionMessage = "This operation is not supported by this object";
		
		[NonSerialized] private static int nextId = 0;
		[NonSerialized] private int id;
		[NonSerialized] private bool isDisposed;
		[NonSerialized] private bool isInitialized;
		[NonSerialized] private bool isFinalized;
		[NonSerialized] private bool isOpenned;


		/// <summary>
		/// 
		///     Inits a Model inctance
		/// 
		/// </summary>
		protected Model()
		{
			Id = nextId;
			nextId++;
		}

		/// <summary>
		/// 
		///     Dispose the object
		/// 
		/// </summary>
		~Model()
		{
			if (!IsDisposed) Dispose();
		}

		[JsonIgnore] public int Id { get => id; private set => id = value; }
		[JsonIgnore] public bool IsDisposed { get => isDisposed; private set => isDisposed = value; }
		[JsonIgnore] public bool IsInitialized { get => isInitialized; private set => isInitialized = value; }
		[JsonIgnore] public bool IsFinalized { get => isFinalized; private set => isFinalized = value; }


		public event PropertyChangingEventHandler PropertyChanging;
		public event PropertyChangedEventHandler PropertyChanged;
		[Obsolete("Use NewDisposing event please")]
		public event EventHandler Disposing;
		[Obsolete("Use NewDisposing event please")]
		public event EventHandler Disposed;
		public event DisposedEventHandler NewDisposed;
		public event DisposingEventHandler NewDisposing;
		[Obsolete("Use NewInitialized event please")]
		public event EventHandler Initialized;
		public event InitializedEventHandler NewInitialized;
		public event InitializingEventHandler NewInitializing;
		public event FinalizedEventHandler Finalized;
		public event FinalizingEventHandler Finalizing;


		protected void OnPropertyChanging([CallerMemberName] string propertyName = "property name") =>
			PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));

		protected void OnPropertyChanged([CallerMemberName] string propertyName = "property name") =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		protected void OpenModel()
		{
			isOpenned = true;
		}

		protected void CloseModel()
		{
			isOpenned = false;
		}

		private void OnInitialized()
		{
			IsInitialized = true;
			Initialized?.Invoke(this, EventArgs.Empty);
			NewInitialized?.Invoke(this, EventArgs.Empty);
		}

		private void OnInitializing() => NewInitializing?.Invoke(this, EventArgs.Empty);

		private void OnDisposed()
		{
			Disposed?.Invoke(this, EventArgs.Empty);
			NewDisposed?.Invoke(this, EventArgs.Empty);
		}

		private void OnDisposing()
		{
			Disposing?.Invoke(this, EventArgs.Empty);
			NewDisposing?.Invoke(this, EventArgs.Empty);
		}

		private void OnFinalized() =>
			Finalized?.Invoke(this, new EventArgs());
		private void OnFinalizing() =>
			Finalizing?.Invoke(this, new EventArgs());


		public virtual object Clone() => throw new NotSupportedException(NotSupportedExceptionMessage);
		
		public void Dispose() { if(IsDisposed == true) throw new InvalidOperationException("Object already disposed"); OnDisposing(); DisposeE(); IsDisposed = true; OnDisposed(); }

		public void FinalizeObject()
		{
			OnFinalizing();


			PropertyChanging += (obj, args) =>
			{
				if(isOpenned == false)
					throw new InvalidOperationException("Object is finalized");
			};

			FinalizeObjectE();

			IsFinalized = true;
			OnFinalized();
		}

		public virtual string ToString(string format, IFormatProvider formatProvider) =>
			throw new NotSupportedException(NotSupportedExceptionMessage);

		public override abstract string ToString();

		public void Formate(object[] args) =>
			Formate(args.Select((s) => s.ToString()).ToArray());

		public virtual void Formate(string[] args) =>
			throw new NotSupportedException(NotSupportedExceptionMessage);

		[Obsolete]
		public virtual void BeginInit() 
		{
			OnInitializing();
		}

		[Obsolete]
		public virtual void EndInit()
		{
			IsInitialized = true;
			OnInitialized();
		}

		public void Initialize()
		{
			OnInitializing();
			InitializeE();
			OnInitialized();
		}

		protected virtual void DisposeE() { }
		protected virtual void FinalizeObjectE() { }
		protected virtual void InitializeE() { }


		protected class ModelOpenHandler : IDisposable
		{
			private readonly Model model;


			public ModelOpenHandler(Model model)
			{
				this.model = model;
				model.OpenModel();
			}


			public void Dispose()
			{
				model.CloseModel();
			}
		}
	}
}

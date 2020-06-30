using StandardLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace StandardLibrary.Models
{
    public abstract class Model : IModel
    {
        public const string NotSupportedExceptionMessage = "This operation is not supported by this object";

        private static int nextId = 0;

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

        public int Id { get; private set; }
        public bool IsDisposed { get; private set; } = false;
        public bool IsInitialized { get; private set; } = false;


        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler Disposing;
        public event EventHandler Disposed;
        public event EventHandler Initialized;


        protected void OnPropertyChanging([CallerMemberName] string propertyName = "property name") =>
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "property name") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void OnInitialized() =>
            Initialized?.Invoke(this, new EventArgs());        
        private void OnDisposed() =>
            Disposed?.Invoke(this, new EventArgs());        
        private void OnDisposing() =>
            Disposing?.Invoke(this, new EventArgs());


        public virtual object Clone() => throw new NotSupportedException(NotSupportedExceptionMessage);
        
        public void Dispose() { OnDisposing(); DisposeE(); IsDisposed = true; OnDisposed(); }

        /// <summary>
        /// 
        ///     IDisposable realisation 
        /// 
        /// </summary>
        protected virtual void DisposeE() { }

        public virtual string ToString(string format, IFormatProvider formatProvider) =>
            throw new NotSupportedException(NotSupportedExceptionMessage);

        public override abstract string ToString();

        public void Formate(object[] args) =>
            Formate(args.Select((s) => s.ToString()).ToArray());

        public virtual void Formate(string[] args) =>
            throw new NotSupportedException(NotSupportedExceptionMessage);

        public virtual void BeginInit() { }

        public virtual void EndInit()
        {
            IsInitialized = true;
            OnInitialized();
        }
    }
}

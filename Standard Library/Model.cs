using Standard_Library.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Standard_Library
{
    public abstract class Model : IModel
    {

        private static int nextId = 0;
        private bool disposed = false;


        protected Model()
        {
            Id = nextId;
            nextId++;
        }


        ~Model()
        {
            if(!disposed) Dispose();
        }


        public int Id { get; }
        public bool Disposed { get; }


        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanging([CallerMemberName] string propertyName = "property name") =>
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "property name") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        

        public abstract object Clone();
        public virtual void Dispose() { disposed = true; }
    }
}

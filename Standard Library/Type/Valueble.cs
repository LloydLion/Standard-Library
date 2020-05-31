using System;
using System.Collections.Generic;
using System.Text;

namespace Standard_Library.TypeManagment
{
    public class Valueble<T> where T : class, ICloneable
    {
        private Valueble() { }

        private T Reference { get; set; }
        public T Value { get => this; }

        public static implicit operator T(Valueble<T> a)
        {
            return (T)a.Reference.Clone();
        }

        public static implicit operator Valueble<T>(T a)
        {
            return new Valueble<T> { Reference = a };
        }
    }
}

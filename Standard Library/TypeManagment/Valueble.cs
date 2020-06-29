using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.TypeManagment
{
    /// <summary>
    /// 
    ///     Do from class struct,
    ///     Nulluble<> but vice versa
    /// 
    /// </summary>
    /// <typeparam name="T">Type for Valuing</typeparam>
    public class Valueble<T> where T : class, ICloneable
    {
        private Valueble() { }

        private T Reference { get; set; }

        /// <summary>
        /// 
        ///     Cloned input value
        /// 
        /// </summary>
        public T Value { get => this; }

        /// <summary>
        /// 
        ///     Analog of a.Value
        /// 
        /// </summary>
        /// <param name="a">Input object</param>
        public static implicit operator T(Valueble<T> a)
        {
            return (T)a.Reference.Clone();
        }

        /// <summary>
        /// 
        ///     Creates Valueble instance 
        /// 
        /// </summary>
        /// <param name="a">Input reference</param>
        public static implicit operator Valueble<T>(T a)
        {
            return new Valueble<T> { Reference = a };
        }
    }
}

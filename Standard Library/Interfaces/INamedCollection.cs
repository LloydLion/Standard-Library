using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Interfaces
{
    public interface INamedCollection<T> : ICollection<T>, IReadOnlyCollection<T>, ICollection where T : INamedObject
    {
        /// <summary>
        /// 
        ///     Gets list element by his name    
        /// 
        /// </summary>
        T GetElementByName(string Name);


        /// <summary>
        /// 
        ///     Search element by his name
        /// 
        /// </summary>
        T this[string index] { get; set; }
    }
}

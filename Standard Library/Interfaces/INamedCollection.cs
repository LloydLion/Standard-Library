using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Interfaces
{
    public interface INamedCollection<T> : ICollection<T>, IReadOnlyCollection<T>, ICollection where T : IPrivateNamedObject
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

        /// <summary>
        /// 
        ///     Checks the sheet for the item with the given name
        /// 
        /// </summary>
        /// <param name="name">Name for check</param>
        /// <returns>Result of checking</returns>
        bool ContainsItemWithEqualsName(string name);
    }
}

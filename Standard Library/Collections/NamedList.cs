using StandardLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StandardLibrary.Collections
{
    /// <summary>
    /// 
    ///     List Collection for named objects
    /// 
    /// </summary>
    public class NamedList<T> : List<T>, INamedList<T> where T : INamedObject  
    {
        /// <summary>
        /// 
        ///     Search element by his name
        /// 
        /// </summary>
        public T this[string index] { get => GetElementByName(index);
            set { var tmp = this.Select((s) => s.Name == index ? value : s).ToArray();
                this.Clear(); this.AddRange(tmp); } }

        /// <summary>
        /// 
        ///     Gets list element by his name    
        /// 
        /// </summary>
        public T GetElementByName(string Name) =>
            this.Where((s) => s.Name == Name).Single();
    }
}

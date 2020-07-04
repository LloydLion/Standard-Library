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
    public class NamedList<T> : List<T>, INamedList<T> where T : IPrivateNamedObject  
    {
        public T this[string index] { get => GetElementByName(index);
            set { var tmp = this.Select((s) => s.Name == index ? value : s).ToArray();
                this.Clear(); this.AddRange(tmp); } }

        public T GetElementByName(string Name) =>
            this.Where((s) => s.Name == Name).Single();

        public new void Add(T item)
		{
			if (ContainsItemWithEqualsName(item.Name)) 
            {
                throw new InvalidOperationException("Item with equals name already exist");
            }

            base.Add(item);
		}

        public bool ContainsItemWithEqualsName(string name)
		{
            return this.Select((s) => s.Name).Contains(name);
		}
    }
}

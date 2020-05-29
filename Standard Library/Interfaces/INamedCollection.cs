using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Standard_Library.Interfaces
{
    public interface INamedCollection<T> : ICollection<T>, IReadOnlyCollection<T>, ICollection where T : INamedObject
    {
        T GetElementByName(string Name);
        T this[string index] { get; set; }
    }
}

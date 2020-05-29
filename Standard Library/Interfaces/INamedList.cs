using System;
using System.Collections.Generic;
using System.Text;

namespace Standard_Library.Interfaces
{
    public interface INamedList<T> : INamedCollection<T>, IList<T> where T : INamedObject
    {

    }
}

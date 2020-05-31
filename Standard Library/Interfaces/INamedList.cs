using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Interfaces
{
    public interface INamedList<T> : INamedCollection<T>, IList<T> where T : INamedObject
    {

    }
}

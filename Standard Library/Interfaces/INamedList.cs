using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Interfaces
{
    ///<summary>
    /// 
    ///     Interface for NamedList<T>
    /// 
    /// </summary>
    public interface INamedList<T> : INamedCollection<T>, IList<T> where T : IPrivateNamedObject
    {

    }
}

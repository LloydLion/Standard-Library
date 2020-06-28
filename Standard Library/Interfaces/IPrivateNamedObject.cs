using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Interfaces
{
    /// <summary>
    /// 
    ///     Interface for named objects
    ///     with readonly Name property
    /// 
    /// </summary>
    public interface IPrivateNamedObject
    {
        string Name { get; }
    }
}

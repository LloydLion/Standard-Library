using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Interfaces
{
    /// <summary>
    /// 
    ///     Interface for named (has Name property) objects
    ///     *Extension from IPrivateNamedObject  
    /// 
    /// </summary>
    public interface INamedObject : IPrivateNamedObject
    {
        void Rename(string newName);
    }
}

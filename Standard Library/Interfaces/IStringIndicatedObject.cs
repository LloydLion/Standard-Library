using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Interfaces
{
    /// <summary>
    /// 
    ///     Interface for indicated with string (has string StringId property) objects
    /// 
    /// </summary>
    public interface IStringIndicatedObject
    {
        /// <summary>
        /// 
        ///     Id of indicated object
        /// 
        /// </summary>
        string StringId { get; }
    }
}

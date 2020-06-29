using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Interfaces
{
    /// <summary>
    /// 
    ///     Interface for indicated (has int Id property) object
    ///     Perfect for models for database
    /// 
    /// </summary>
    public interface INumberIndicatedObject
    {
        /// <summary>
        /// 
        ///     Numerical indicator of object
        /// 
        /// </summary>
        int Id { get; }
    }
}

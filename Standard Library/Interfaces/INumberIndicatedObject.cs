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
        int Id { get; }
    }
}

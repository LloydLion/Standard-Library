using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Interfaces
{
    /// <summary>
    /// 
    ///     Interface for objects with string type properties 
    /// 
    /// </summary>
    public interface IPropertyFormattable
    {
        /// <summary>
        /// 
        ///     Formate object with object[] args
        ///     Analog with Formate(string[]):
        ///     obj.Formate(args.Select((s) => s.ToString()).ToArray())
        /// 
        /// </summary>
        void Formate(object[] args);

        /// <summary>
        /// 
        ///     Formate object with string[] args
        /// 
        /// </summary>
        void Formate(string[] args);
    }
}

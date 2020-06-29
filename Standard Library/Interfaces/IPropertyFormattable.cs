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
        ///    Formate object with object[] args
        ///    Analog with Formate(string[]):
        ///    obj.Formate(args.Select((s) => s.ToString()).ToArray())
        /// 
        /// </summary>
        /// <param name="args">Objects to formate</param>
        void Formate(object[] args);


        /// <summary>
        /// 
        ///     Formate object with string[] args
        /// 
        /// </summary>
        /// <param name="args">String parameters of formate</param>
        void Formate(string[] args);
    }
}

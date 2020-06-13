using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Interfaces
{
    /// <summary>
    /// 
    ///     Interface for objects supports Disposing
    ///     (implements IDisposable interface)
    ///
    /// </summary>
    public interface IDisposedInformator
    {
        /// <summary>
        /// 
        ///     If object invokes a Dispose() return true
        ///     Else false
        /// 
        /// </summary>
        bool IsDisposed { get; }


        /// <summary>
        /// 
        ///     Invokes on object start Disposing 
        ///     (before Dispose() method invokes, but his work hasn't started)     
        /// 
        /// </summary>
        event EventHandler Disposing;

        /// <summary>
        ///
        ///     Invokes on object end Disposing and do it sucsessfully
        ///     (before Dispose() method invokes and his work has complite)
        /// 
        /// </summary>
        event EventHandler Disposed;
    }
}

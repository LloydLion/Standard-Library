using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Interfaces
{
    public interface IDisposedInformator
    {
        bool IsDisposed { get; }

        event EventHandler Disposing;
        event EventHandler Disposed;
    }
}

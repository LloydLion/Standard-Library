using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Interfaces
{
    public interface INamedObject : IPrivateNamedObject
    {
        void Rename(string newName);
    }
}

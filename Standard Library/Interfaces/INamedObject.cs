using System;
using System.Collections.Generic;
using System.Text;

namespace Standard_Library.Interfaces
{
    public interface INamedObject : IPrivateNamedObject
    {
        void Rename(string newName);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Standard_Library.Interfaces
{
    public interface IPropertyFormattable
    {
        void Formate(object[] args);

        void Formate(string[] args);
    }
}

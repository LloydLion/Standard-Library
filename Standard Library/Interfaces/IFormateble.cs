using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Interfaces
{
    public interface IPropertyFormattable
    {
        void Formate(object[] args);

        void Formate(string[] args);
    }
}

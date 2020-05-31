using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Standard_Library.Interfaces
{
    public interface IModel : INotifyPropertyChanging, INotifyPropertyChanged, ICloneable, 
        IDisposable, INumberIndicatedObject, IDisposedInformator
    {

    }
}

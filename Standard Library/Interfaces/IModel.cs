using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StandardLibrary.Interfaces
{
    public interface IModel : INotifyPropertyChanging, INotifyPropertyChanged, ICloneable, 
        IDisposable, INumberIndicatedObject, IDisposedInformator
    {

    }
}

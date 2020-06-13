using StandardLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StandardLibrary.Interfaces
{
    /// <summary>
    /// 
    ///     Interface for models
    /// 
    /// </summary>
    public interface IModel : INotifyPropertyChanging, INotifyPropertyChanged, ICloneable, 
        IDisposable, INumberIndicatedObject, IDisposedInformator, IFormattable, IPropertyFormattable, 
        ISupportInitializeNotification
    {

    }
}

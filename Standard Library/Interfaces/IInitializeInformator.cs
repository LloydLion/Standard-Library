using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StandardLibrary.Interfaces
{
	public delegate void InitializedEventHandler(object invoker, EventArgs args);
	public delegate void InitializingEventHandler(object invoker, EventArgs args);


	/// <summary>
	/// 
	///		Informates for object initialize
	/// 
	/// </summary>
	public interface IInitializeInformator : IInitializebe
	{
		/// <summary>
		/// 
		///		Invokes on object has initialized
		/// 
		/// </summary>
		event InitializedEventHandler NewInitialized;

		/// <summary>
		/// 
		///		Invokes on object is starting of initializing
		/// 
		/// </summary>
		event InitializingEventHandler NewInitializing;
	}
}

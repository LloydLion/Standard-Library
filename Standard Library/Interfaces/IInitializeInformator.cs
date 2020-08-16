using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StandardLibrary.Interfaces
{
	public delegate void InitializedEventHandler(object invoker, EventArgs args);
	public delegate void InitializingEventHandler(object invoker, EventArgs args);

	public interface IInitializeInformator : IInitializebe
	{
		event InitializedEventHandler NewInitialized;
		event InitializingEventHandler NewInitializing;
	}
}

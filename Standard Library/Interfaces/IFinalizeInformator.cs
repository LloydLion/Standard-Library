using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Interfaces
{
	public delegate void FinalizedEventHandler(object invoker, EventArgs args);
	public delegate void FinalizingEventHandler(object invoker, EventArgs args);

	/// <summary>
	/// 
	///		Informate for Finalize method invoke in IFinalizeble object
	/// 
	/// </summary>
	public interface IFinalizeInformator : IFinalizeble
	{
		/// <summary>
		/// 
		///		Invokes on object finalized
		/// 
		/// </summary>
		event FinalizedEventHandler Finalized;


		/// <summary>
		/// 
		///		Invokes on object begin finalizing
		/// 
		/// </summary>
		event FinalizingEventHandler Finalizing;
	}
}

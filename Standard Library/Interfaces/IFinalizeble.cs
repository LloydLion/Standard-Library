using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.Interfaces
{
	/// <summary>
	/// 
	///		Interface for finalizeble object for fixate corrent properties values
	/// 
	/// </summary>
	public interface IFinalizeble
	{
		/// <summary>
		/// 
		///		Gets finalize status
		/// 
		/// </summary>
		bool IsFinalized { get; }

		/// <summary>
		/// 
		///		finalize object (make all properties readonly)
		/// 
		/// </summary>
		void FinalizeObject();
	}
}

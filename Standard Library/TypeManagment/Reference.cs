using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.TypeManagment
{
	public class Reference<T> where T : struct 
	{
		public T Value { get; }

		public Reference(T value)
		{
			Value = value;
		}
	}
}

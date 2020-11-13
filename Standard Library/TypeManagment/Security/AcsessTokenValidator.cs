using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.TypeManagment.Security
{
	public class AcsessTokenValidator
	{
		private readonly AcsessToken target;


		public AcsessTokenValidator(AcsessToken token)
		{
			target = token;
		}


		public bool IsValid(AcsessToken check) => check == target;
	}
}

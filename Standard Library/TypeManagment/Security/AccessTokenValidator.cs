using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.TypeManagment.Security
{
	public class AccessTokenValidator
	{
		private readonly AccessToken target;


		public AccessTokenValidator(AccessToken token)
		{
			target = token;
		}


		public bool IsValid(AccessToken check) => check == target;
	}
}

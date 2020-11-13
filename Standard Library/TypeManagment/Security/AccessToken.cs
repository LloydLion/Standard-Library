using StandardLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.TypeManagment.Security
{
	public class AccessToken : INumberIndicatedObject
	{
		private static int nextId = 0;


		public AccessToken()
		{
			Id = ++nextId;
		}


		public int Id { get; }


		public static bool operator ==(AccessToken left, AccessToken right) => left.Id == right.Id;
		public override bool Equals(object obj) => obj is AccessToken token && this == token;
		public override int GetHashCode() => 2108858624 + Id.GetHashCode();
		public static bool operator !=(AccessToken left, AccessToken right) => !(left == right);
	}
}

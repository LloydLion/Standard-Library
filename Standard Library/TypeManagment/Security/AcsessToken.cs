using StandardLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StandardLibrary.TypeManagment.Security
{
	public class AcsessToken : INumberIndicatedObject
	{
		private static int nextId = 0;


		public AcsessToken()
		{
			Id = ++nextId;
		}


		public int Id { get; }


		public static bool operator ==(AcsessToken left, AcsessToken right) => left.Id == right.Id;
		public override bool Equals(object obj) => obj is AcsessToken token && this == token;
		public override int GetHashCode() => 2108858624 + Id.GetHashCode();
		public static bool operator !=(AcsessToken left, AcsessToken right) => !(left == right);
	}
}

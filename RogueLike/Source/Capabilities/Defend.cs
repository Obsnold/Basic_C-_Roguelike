using System;

namespace RogueLike
{
	public class Defend
	{
		public readonly int Str;
		public readonly int Dex;
		public readonly int Int;

		public Defend (int aStr = 0, int aDex = 0, int aInt = 0)
		{
			this.Str = aStr;
			this.Dex = aDex;
			this.Int = aInt;
		}
	}
}


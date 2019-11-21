using System;

namespace RogueLike
{
	public class Defend
	{
		public readonly int Con;
		public readonly int Ref;
		public readonly int Wis;

		public Defend (int aCon = 0, int aRef = 0, int aWis = 0)
		{
			this.Con = aCon;
			this.Ref = aRef;
			this.Wis = aWis;
		}
	}
}


using System;

namespace RogueLike
{
	public class StrengthUse : Use
	{
		int Strength;
		public StrengthUse (int aStrength = 1)
		{
			this.Strength = aStrength;
		}

		public bool use(Actor aActor){
			aActor.Strength += this.Strength;
			return true;
		}
	}
}


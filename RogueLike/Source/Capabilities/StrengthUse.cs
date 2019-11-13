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
			aActor.Stats.Str += this.Strength;
			return true;
		}
	}
}


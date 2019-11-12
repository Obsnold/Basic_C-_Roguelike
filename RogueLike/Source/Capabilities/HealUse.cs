using System;

namespace RogueLike
{
	public class HealUse : Use
	{
		int Regen;
		public HealUse (int aRegen = 1)
		{
			this.Regen = aRegen;
		}

		public bool use(Actor aActor){
			aActor.Health += this.Regen;
			return true;
		}
	}
}


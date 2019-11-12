using System;

namespace RogueLike
{
	public class PoisonUse : Use
	{
		int Damage;
		public PoisonUse (int aDamage = 1)
		{
			this.Damage = aDamage;
		}

		public bool use(Actor aActor){
			aActor.Health -= this.Damage;
			return true;
		}
	}
}


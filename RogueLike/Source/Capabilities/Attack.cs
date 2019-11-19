using System;

namespace RogueLike
{
	public class Attack
	{
		int minDamage;
		int maxDamage;
		
		public Attack(int aMinDamage = 0, int aMaxDamage = 0)
		{
			this.minDamage = aMinDamage;
			this.maxDamage = aMaxDamage;
		}

		public int CalculateDamage (){
			return StaticRandom.Instance.Next (this.minDamage, this.maxDamage);
		}
	}
}


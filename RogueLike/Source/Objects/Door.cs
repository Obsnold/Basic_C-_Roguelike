using System;

namespace RogueLike
{
	public class Door : ObjectInterface
	{
		bool IsOpen = false;
		int Health  = 10;
		int Toughness = 0;
		public Door ()
		{
		}

		public bool Interact(Creature aCreature){
			IsOpen = !IsOpen;
			return true;
		}

		public bool TakeDamage(int aDamage){
			bool lIsDestroyed = false;
			this.Health -= aDamage;
			if (this.Health < 0) {
				lIsDestroyed = true;
			}
			return lIsDestroyed;
		}

		public bool CanWalk(){
			return IsOpen;
		}

		public bool CanSeePast(){
			return IsOpen;
		}
	}
}


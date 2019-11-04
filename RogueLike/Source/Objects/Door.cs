using System;

namespace RogueLike
{
	public class Door : ObjectInterface
	{
		bool IsOpen = false;
		int Health  = 10;
		int Toughness = 0;
		public Door (bool aIsOpen = false, int aHealth = 10, int aToughness = 0)
		{
			IsOpen = aIsOpen;
			Health  = aHealth;
			Toughness = aToughness;
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

		public DisplayTile GetDisplayTile(){
			DisplayTile lTile = DisplayTile.DoorClosed;
			if(this.IsOpen){
				lTile = DisplayTile.DoorOpen;
			}
			return lTile;
		}
	}
}


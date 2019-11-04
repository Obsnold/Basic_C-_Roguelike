using System;

namespace RogueLike
{
	public class Creature{
		public Coordinate pos;
		public int Health;
		public int Strength;
		public int Group;
		public int Vision = 8;
		public String Name;

		public Creature(String aName, int aX, int aY, int aHealth ,int aStrength, int aGroup){
			this.Name = aName;
			this.pos.x = aX;
			this.pos.y = aY;
			this.Health = aHealth;
			this.Strength = aStrength;
			this.Group = aGroup;
		}

		public bool TakeDamage(int aDamage){
			this.Health -= aDamage;
			return IsDead ();
		}

		public bool IsDead(){
			bool isDead = false;
			if (this.Health <= 0) {
				isDead = true;
			}
			return isDead;
		}
	}
}


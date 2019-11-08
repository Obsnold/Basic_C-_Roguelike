using System;
using System.Collections.Generic;

namespace RogueLike 
{
	public class Actor
	{
		public Coordinate pos;
		public int Health;
		public int Strength;
		public int Group;
		public int Vision = 8;
		public String Name;
		public List<ItemInterface> Inventory;
		public Level Level;

		public Actor(String aName, int aX, int aY, int aHealth ,int aStrength, int aGroup, Level aLevel){
			this.Name = aName;
			this.pos.x = aX;
			this.pos.y = aY;
			this.Health = aHealth;
			this.Strength = aStrength;
			this.Group = aGroup;
			this.Inventory = new List<ItemInterface> ();
			this.Level = aLevel;
		}

		public virtual Action TakeTurn (){
			return null;
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


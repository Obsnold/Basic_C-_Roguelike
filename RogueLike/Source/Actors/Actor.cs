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
		Dungeon Dungeon = Dungeon.Instance;

		public Actor(String aName, int aX, int aY, int aHealth ,int aStrength, int aGroup){
			this.Name = aName;
			this.pos.x = aX;
			this.pos.y = aY;
			this.Health = aHealth;
			this.Strength = aStrength;
			this.Group = aGroup;
			this.Inventory = new List<ItemInterface> ();
			this.Level = this.Dungeon.GetCurrentLevel();
		}

		public virtual Action TakeTurn (){
			return null;
		}

		public bool SetPos(Coordinate aCoord){
			this.Level = this.Dungeon.GetCurrentLevel();
			bool result = false;
			if(this.Level.InBounds(aCoord.x,aCoord.y) &&
				(!this.Level.PathBlocked(aCoord.x,aCoord.y)))
			{
				this.Level.ActorGrid.SetItem (null, this.pos);
				this.pos = aCoord;
				this.Level.ActorGrid.SetItem (this, this.pos);
				result = true;
			}
			return result;
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


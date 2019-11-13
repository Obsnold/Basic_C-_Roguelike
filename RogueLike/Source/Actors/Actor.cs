using System;
using System.Collections.Generic;

namespace RogueLike 
{
	public class Actor
	{
		protected Coordinate pos;
		protected Level level;
		protected Dungeon dungeon = Dungeon.Instance;

		public ActorTemplate Stats;
		public String Name;
		public List<Item> Inventory;
		public int Health;
		public int Group;
		public int Vision = 8;


		public Actor(String aName, ActorTemplate aTemplate, int aX, int aY, int aGroup){
			this.Name = aName;
			this.Stats = aTemplate;
			this.Inventory = new List<Item> ();
			this.Group = aGroup;
			this.pos.x = aX;
			this.pos.y = aY;
			this.Health = this.Stats.MaxHealth;
		}

		public Coordinate GetPos(){
			return this.pos;
		}

		public bool SetPos(Coordinate aCoord){
			this.level = this.dungeon.GetCurrentLevel();
			bool result = false;
			if(this.level.InBounds(aCoord.x,aCoord.y) &&
				(!this.level.PathBlocked(aCoord.x,aCoord.y)))
			{
				this.level.ActorGrid.SetItem (null, this.pos);
				this.pos = aCoord;
				this.level.ActorGrid.SetItem (this, this.pos);
				result = true;
			}
			return result;
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


using System;
using System.Collections.Generic;

namespace RogueLike 
{
	public class Actor
	{
		protected Coordinate pos;
		protected Level level;
		protected Dungeon dungeon = Dungeon.Instance;

		public Dictionary<String,ActorBodyPart> Body;

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

			Body = new Dictionary<string, ActorBodyPart> ();
			Body.Add("LeftHand",new ActorBodyPart("LeftHand"));
			Body.Add("RightHand",new ActorBodyPart("RightHand"));
			Body.Add("Head",new ActorBodyPart("Head"));
			Body.Add("Torso",new ActorBodyPart("Torso"));
			Body.Add("Legs",new ActorBodyPart("Legs"));
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

		public int GetDefBonus(){

			return 1;
		}

		public bool Equip(Item aItem){
			bool lResult = true;
			if (aItem.EquipTo != null) {
				if (Body [aItem.EquipTo].Equiped != null) {
					Inventory.Add (Body [aItem.EquipTo].Equiped);
				}
				lResult = Body [aItem.EquipTo].Equip(aItem);
				Inventory.Remove (aItem);
			} 
			return lResult;
		}

	}
}


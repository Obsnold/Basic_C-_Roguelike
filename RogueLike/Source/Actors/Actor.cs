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

		public int GetMeleeDefence(){
			int lBonus = this.Stats.Con;
			foreach (String aPart in Body.Keys) {
				if (Body [aPart].Equiped != null) {
					if (Body [aPart].Equiped.defend != null) {
						lBonus += Body [aPart].Equiped.defend.Con;
					}
				}
			}
			return lBonus;
		}

		public bool MeleeRollToHit(Actor aActor){
			bool lResult = false;
			int lRoll = this.Stats.Str + StaticRandom.Instance.Next (0, 100);

			if (lRoll >= aActor.GetMeleeDefence()) {
				lResult = true;
			}

			return lResult;
		}

		public int MeleeDamage(){
			int lDamage = 0;
			foreach (String aPart in Body.Keys) {
				if (Body [aPart].Equiped != null) {
					if (Body [aPart].Equiped.attack != null) {
						lDamage += Body [aPart].Equiped.attack.CalculateDamage ();
					}
				}
			}
			if (lDamage == 0) {
				lDamage = 1;
			}
			return lDamage;
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

		public bool AttributeCheck(int aBaseStat, int aCheckValue, int aModifier = 0){
			int lResult = StaticRandom.Instance.Next (0,100);
			if ((lResult + aBaseStat + aModifier) >= aCheckValue) {
				return true;
			} else {
				return false;
			}
		}
	}
}


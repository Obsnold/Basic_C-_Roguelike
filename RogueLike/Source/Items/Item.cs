using System;
using System.Collections.Generic;

namespace RogueLike
{
	public class Item
	{
		Coordinate pos;
		Level level;
		Dungeon dungeon = Dungeon.Instance;

		String name;
		public Tags Tags;
		public String EquipTo;

		public Attack attack;
		public Defend defend;
		public Use use;

		public Item(String aName, Attack aAttack = null, Defend aDefend = null, Use aUse = null, String aEquipTo = null){
			this.name = aName;
			this.attack = aAttack;
			this.defend = aDefend;
			this.use = aUse;
			this.Tags = new Tags ();
			EquipTo = aEquipTo;
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
				this.level.ItemGrid.SetItem (null, this.pos);
				this.pos = aCoord;
				this.level.ItemGrid.SetItem (this, this.pos);
				result = true;
			}
			return result;
		}
		
		public String GetName(){
			return this.name;
		}
	}
}


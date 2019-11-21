using System;

namespace RogueLike
{
	public class ItemGenerator
	{
		ConfigData Config;
		public ItemGenerator ()
		{
			Config = new ConfigData ();
		}

		public bool PopulateLevel(Level aLevel){
			Item lItem;
			Coordinate lCoord;
			//Generate Items
			for (int i = 0; i < 5; i++) {
				room lTempRoom = aLevel.RoomList[StaticRandom.Instance.Next(1,aLevel.RoomList.Count)];

				int lPotionType = StaticRandom.Instance.Next (0, 100);
				if (lPotionType < 33) {
					lItem = new Item ("Poison", aUse: new PoisonUse ());
				} else if ( lPotionType < 66){
					lItem = new Item ("Health Potion", aUse: new HealUse ());
				} else {
					lItem = new Item ("Str Potion", aUse: new StrengthUse ());
				}

				lCoord.x = StaticRandom.Instance.Next (lTempRoom.x, lTempRoom.x + lTempRoom.w);
				lCoord.y = StaticRandom.Instance.Next (lTempRoom.y, lTempRoom.y + lTempRoom.h);
				lItem.SetPos (lCoord);
				//this.Level.ItemGrid.SetItem (lItem,lItem.GetPos());
			}

			lCoord = aLevel.Player.GetPos();
			//create starting items
			lItem = this.Config.ItemDic["Sword"];
			lCoord.x += 1;
			lItem.SetPos (lCoord);
			lItem = this.Config.ItemDic["Shield"];
			lCoord.x -= 2;
			lItem.SetPos (lCoord);
			lItem = this.Config.ItemDic["Chain Mail"];
			lCoord.y -= 1;
			lItem.SetPos (lCoord);
			lItem = this.Config.ItemDic["Helmet"];
			lCoord.y += 2;
			lItem.SetPos (lCoord);
			lItem = this.Config.ItemDic["Boots"];
			lCoord.x += 2;
			lItem.SetPos (lCoord);
			return true;
		}

		/*public bool AddItemToInventory(){



		}*/
	}
}


using System;
using System.Collections.Generic;

namespace RogueLike
{


	public class LevelGenerator
	{
		int MinRoomSize = 1;
		int MaxRoomSize = 10;
		int LevelWidth = 40;
		int LevelHeight = 40;
		List<string> Names;
		Level Level;

		public LevelGenerator (int aWidth, int aHeight)
		{
			this.LevelHeight = aHeight;
			this.LevelWidth = aWidth;
			Level = new Level(aWidth,aHeight);
			this.Names = new List<string> ();
			this.Names.Add ("Barry");
			this.Names.Add ("Harold");
			this.Names.Add ("Malcom");
			this.Names.Add ("Jeff");
			this.Names.Add ("Jeoff");
			this.Names.Add ("Tim");
			this.Names.Add ("Michael");
			this.Names.Add ("Paul");
			this.Names.Add ("Wally");
		}

		public bool genLevel(){
			//clear level
			clearLevel();

			//place first room
			room tempRoom = new room();
			tempRoom.x = (this.LevelWidth/2)-1;
			tempRoom.y = (this.LevelHeight/2)-1;
			tempRoom.h = 3;
			tempRoom.w = 3;
			if (placeRoom (tempRoom)) {
				this.Level.RoomList.Add (tempRoom);
				this.Level.BaseGrid.SetItem(LevelTiles.Ladder,tempRoom.x+1,tempRoom.y+1);
			}

			//Generate Rooms

			for (int i = 0;i < 100;i ++) {
				int roomNum = StaticRandom.Instance.Next (0, this.Level.RoomList.Count);

				tempRoom.h = StaticRandom.Instance.Next (this.MinRoomSize, this.MaxRoomSize);
				tempRoom.w = StaticRandom.Instance.Next (this.MinRoomSize, this.MaxRoomSize);

				int tempX = 0;
				int tempY = 0;
				switch(StaticRandom.Instance.Next(0,4)){
				case 0://north
					//debugPrint ("North-------------------");
					tempX = StaticRandom.Instance.Next(this.Level.RoomList [roomNum].x,this.Level.RoomList [roomNum].x+this.Level.RoomList [roomNum].w);
					tempY = this.Level.RoomList [roomNum].y - 1;
					tempRoom.x = tempX - StaticRandom.Instance.Next(0,tempRoom.w);
					tempRoom.y = tempY - tempRoom.h;
					break;
				case 1://south
					//debugPrint ("South-----------------");
					tempX = StaticRandom.Instance.Next(this.Level.RoomList [roomNum].x,this.Level.RoomList [roomNum].x+this.Level.RoomList [roomNum].w);
					tempY = this.Level.RoomList [roomNum].y + this.Level.RoomList [roomNum].h;
					tempRoom.x = tempX - StaticRandom.Instance.Next(0,tempRoom.w);
					tempRoom.y = tempY + 1;
					break;
				case 2://east
					//debugPrint ("East-------------------");
					tempX = this.Level.RoomList [roomNum].x + this.Level.RoomList [roomNum].w;
					tempY = StaticRandom.Instance.Next(this.Level.RoomList [roomNum].y,this.Level.RoomList [roomNum].y+this.Level.RoomList [roomNum].h);
					tempRoom.x = tempX + 1;
					tempRoom.y = tempY - StaticRandom.Instance.Next(0,tempRoom.h);
					break;
				case 3://west
					//debugPrint ("West--------------------");
					tempX = this.Level.RoomList [roomNum].x - 1;
					tempY = StaticRandom.Instance.Next(this.Level.RoomList [roomNum].y,this.Level.RoomList [roomNum].y+this.Level.RoomList [roomNum].h);
					tempRoom.x = tempX - tempRoom.w;
					tempRoom.y = tempY - StaticRandom.Instance.Next(0,tempRoom.h);
					break;
				}
				if (placeRoom (tempRoom)) {
					this.Level.BaseGrid.SetItem(LevelTiles.Floor, tempX, tempY);
					this.Level.ObjectGrid.SetItem(new Door(),tempX,tempY);
				}
			}

			return true;
		}
		/*
		public bool genItems(Level aLevel){
			Item lItem;
			Coordinate lCoord;
			//Generate Items
			for (int i = 0; i < 5; i++) {
				room lTempRoom = aLevel.RoomList[StaticRandom.Instance.Next(1,this.Level.RoomList.Count)];

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

			lCoord.x = this.LevelWidth / 2;
			lCoord.y = this.LevelHeight/2;
			//create starting items
			lItem = new Item("Sword", aAttack : new Attack(2,4), aEquipTo:"RightHand");
			lCoord.x += 1;
			lItem.SetPos (lCoord);
			lItem = new Item("Sheild", aDefend : new Defend(2), aEquipTo:"LeftHand");
			lCoord.x -= 2;
			lItem.SetPos (lCoord);
			lItem = new Item("Helmet", aDefend : new Defend(2), aEquipTo:"Head");
			lCoord.y -= 1;
			lItem.SetPos (lCoord);
			lItem = new Item("Chain Mail", aDefend : new Defend(2), aEquipTo:"Torso");
			lCoord.y += 2;
			lItem.SetPos (lCoord);
			lItem = new Item("Boots", aDefend : new Defend(2), aEquipTo:"Legs");
			lCoord.x += 2;
			lItem.SetPos (lCoord);
			return true;
		}*/


		public Level getLevel(){
			if (this.Level != null) {
				return this.Level;
			} else {
				return null;
			}
		}

		bool placeRoom(room aRoom){
			bool roomPlaced = false;
			//debugPrint ("placeRoom: x=" + aRoom.x + ", y=" + aRoom.y + ", h=" + aRoom.h + ", w=" + aRoom.w);
			if ((aRoom.x + aRoom.w >= this.LevelWidth-1) || (aRoom.y + aRoom.h >= this.LevelHeight-1)
				|| (aRoom.x <= 0) || (aRoom.y <= 0)){
				roomPlaced = false;
			} else {
				roomPlaced = true;
			}
			if (roomPlaced) {
				for (int y = aRoom.y - 1; y < aRoom.y + aRoom.h + 1; y++) {
					for (int x = aRoom.x - 1; x < aRoom.x + aRoom.w + 1; x++) {
						if (this.Level.BaseGrid.GetItem(x,y) == LevelTiles.Floor) {
							roomPlaced = false;
							break;
						}
					}
				}
			}
			if (roomPlaced) {
				for (int y = aRoom.y; y < aRoom.y+aRoom.h; y++) {
					for (int x = aRoom.x; x < aRoom.x+aRoom.w; x++) {
						this.Level.BaseGrid.SetItem(LevelTiles.Floor,x,y);
					}
				}
			}
			if (roomPlaced) {
				this.Level.RoomList.Add (aRoom);
			}
			return roomPlaced;
		}

		void clearLevel(){
			for (int y = 0; y < this.LevelHeight; y++) {
				for (int x = 0; x < this.LevelWidth; x++) {
					this.Level.BaseGrid.SetItem(LevelTiles.Wall,x,y);
				}
			}
		}
			
	}
}



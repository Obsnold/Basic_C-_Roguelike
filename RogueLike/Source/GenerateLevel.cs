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
		Random Rand;
		Level Level;

		public LevelGenerator (int aWidth, int aHeight)
		{
			this.LevelHeight = aHeight;
			this.LevelWidth = aWidth;
			Level = new Level(aWidth,aHeight);
			this.Rand = new Random ();
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
				int roomNum = this.Rand.Next (0, this.Level.RoomList.Count);

				tempRoom.h = this.Rand.Next (this.MinRoomSize, this.MaxRoomSize);
				tempRoom.w = this.Rand.Next (this.MinRoomSize, this.MaxRoomSize);

				int tempX = 0;
				int tempY = 0;
				switch(this.Rand.Next(0,4)){
				case 0://north
					//debugPrint ("North-------------------");
					tempX = this.Rand.Next(this.Level.RoomList [roomNum].x,this.Level.RoomList [roomNum].x+this.Level.RoomList [roomNum].w);
					tempY = this.Level.RoomList [roomNum].y - 1;
					tempRoom.x = tempX - this.Rand.Next(0,tempRoom.w);
					tempRoom.y = tempY - tempRoom.h;
					break;
				case 1://south
					//debugPrint ("South-----------------");
					tempX = this.Rand.Next(this.Level.RoomList [roomNum].x,this.Level.RoomList [roomNum].x+this.Level.RoomList [roomNum].w);
					tempY = this.Level.RoomList [roomNum].y + this.Level.RoomList [roomNum].h;
					tempRoom.x = tempX - this.Rand.Next(0,tempRoom.w);
					tempRoom.y = tempY + 1;
					break;
				case 2://east
					//debugPrint ("East-------------------");
					tempX = this.Level.RoomList [roomNum].x + this.Level.RoomList [roomNum].w;
					tempY = this.Rand.Next(this.Level.RoomList [roomNum].y,this.Level.RoomList [roomNum].y+this.Level.RoomList [roomNum].h);
					tempRoom.x = tempX + 1;
					tempRoom.y = tempY - this.Rand.Next(0,tempRoom.h);
					break;
				case 3://west
					//debugPrint ("West--------------------");
					tempX = this.Level.RoomList [roomNum].x - 1;
					tempY = this.Rand.Next(this.Level.RoomList [roomNum].y,this.Level.RoomList [roomNum].y+this.Level.RoomList [roomNum].h);
					tempRoom.x = tempX - tempRoom.w;
					tempRoom.y = tempY - this.Rand.Next(0,tempRoom.h);
					break;
				}
				if (placeRoom (tempRoom)) {
					this.Level.BaseGrid.SetItem(LevelTiles.Floor, tempX, tempY);
					this.Level.ObjectGrid.SetItem(new Door(),tempX,tempY);
				}
			}

			//Generate Enemys
			//Console.SetCursorPosition (1, 45);
			for (int i = 0; i < 5; i++) {
				room lTempRoom = this.Level.RoomList[this.Rand.Next(1,this.Level.RoomList.Count)];
				Actor lActor = new Enemy (this.Names[this.Rand.Next(0,this.Names.Count)],this.Rand.Next (lTempRoom.x, lTempRoom.x + lTempRoom.w),
					this.Rand.Next (lTempRoom.y, lTempRoom.y + lTempRoom.h), 4, 1, 1, this.Level);
				if (!this.Level.moveActor (lActor, lActor.pos.x, lActor.pos.y)) {
					//debugPrint ("Cannot place Actor");
				}
			}
			//Generate Items
			for (int i = 0; i < 5; i++) {
				room lTempRoom = this.Level.RoomList[this.Rand.Next(1,this.Level.RoomList.Count)];
				ItemInterface lItem = new Potion ();
				this.Level.ItemGrid.SetItem (lItem, this.Rand.Next (lTempRoom.x, lTempRoom.x + lTempRoom.w),
					this.Rand.Next (lTempRoom.y, lTempRoom.y + lTempRoom.h));
			}


			return true;
		}

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



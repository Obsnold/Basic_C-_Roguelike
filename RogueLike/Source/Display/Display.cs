using System;

namespace RogueLike
{
	public class Display
	{
		private static Display mDisplay;

		int ScreenWidth = 80;
		int ScreenHeight = 24;

		int MapPosX = 1;
		int MapPosY = 1;
		int MapWidth = 48;
		int MapHeight = 22;

		int StatPosX = 51;
		int StatPosY = 1;
		int StatWidth = 28;
		int StatHeight = 10;

		int HistPosX = 51;
		int HistPosY = 12;
		int HistWidth = 28;
		int HistHeight = 11;

		String title = "Rogue Like";
		char[,] Screen;
		char tBorderH = '-';
		char tBorderV = '|';
		char tBorderC = '+';
		char tFloor = '.';
		char tLadder = 'L';
		char tDoorOpen = 'd';
		char tDoorClosed = 'D';
		char tWall = '#';
		char tPlayer = '@';
		char tEnemy = 'e';
		char tSelection = 'X';
		char tItem = 'i';

		public static Display Instance
		{
			get{
				if (mDisplay == null) {
					mDisplay = new Display ();
				} 
				return mDisplay;
			}
		}

		public Display ()
		{
			try{
				Console.Title = title;
				Console.Clear();
				Console.CursorVisible = false;
				this.Screen = new char[this.ScreenWidth,this.ScreenHeight];
			} catch (Exception ex)
			{
				Console.WriteLine("Exception initalising display:" + ex.ToString());
			}
		}

		public void updateScreen(){
			for (int y = 0; y < this.ScreenHeight; y++) {
				for (int x = 0; x < this.ScreenWidth; x++) {
					Console.SetCursorPosition (x, y);
					Console.Write (this.Screen[x,y]);
				}
			}
		}

		public void printMainMenu(){
			clearArea (0, 0, this.ScreenWidth, this.ScreenWidth);
			printBorder (0, 0, this.ScreenWidth, this.ScreenHeight);
			printBorder (0, 0, this.ScreenWidth, this.ScreenHeight);
			updateScreen ();
		}

		public void printMainScreen(Level aLevel){
			clearArea (0, 0, this.ScreenWidth, this.ScreenWidth);
			printBorder (0, 0, this.ScreenWidth, this.ScreenHeight); // main border
			printBorder (this.HistPosX -1 , this.HistPosY -1, this.HistWidth + 2, this.HistHeight+2); // history
			printBorder (this.StatPosX -1, this.StatPosY -1, this.StatWidth +2, this.StatHeight+2); //stats
			printMap (aLevel);
			printStats (aLevel);
			printHistory (aLevel);
			updateScreen ();
		}
			

		public void printMap(Level aLevel){
			char lTempChar = ' ';
			int lY = aLevel.Player.GetPos().y - aLevel.Player.Vision;
			int lX = aLevel.Player.GetPos().x - aLevel.Player.Vision;
			for (int y = 0; y < (aLevel.Player.Vision * 2) + 1; y++){
				int lTY = lY + y;
				for (int x = 0; x < (aLevel.Player.Vision * 2) + 1; x++){
					int lTX = lX + x;
					if ( !aLevel.InBounds(lTX,lTY) ) {
						printChar (' ', x + this.MapPosX, y + this.MapPosY);
					} else {
						if (aLevel.VisibilityGrid.GetItem(lTX, lTY)) {
							if (aLevel.ActorGrid.GetItem (lTX, lTY) != null) {
								lTempChar = tEnemy;
							} else if (aLevel.ItemGrid.GetItem (lTX, lTY) != null) {
								lTempChar = tItem;
							} else {
								switch (aLevel.BaseGrid.GetItem(lTX, lTY)) {
								case LevelTiles.Floor:
									lTempChar = tFloor;
									break;
								case LevelTiles.Wall:
									lTempChar = tWall;
									break;
								case LevelTiles.Ladder:
									lTempChar = tLadder;
									break;
								}

								if (aLevel.ObjectGrid.GetItem(lTX, lTY) != null) {
									switch (aLevel.ObjectGrid.GetItem(lTX, lTY).GetDisplayTile()) {
									case DisplayTile.DoorClosed:
										lTempChar = tDoorClosed;
										break;
									case DisplayTile.DoorOpen:
										lTempChar = tDoorOpen;
										break;
									}

								}
							}
							printChar (lTempChar, 
								x + this.MapPosX + ((this.MapWidth/2)-aLevel.Player.Vision), 
								y + this.MapPosY + ((this.MapHeight/2)-aLevel.Player.Vision));
						}
					}
				}
			}

			Coordinate lPlayer;
			lPlayer.x = aLevel.Player.Vision + this.MapPosX + ((this.MapWidth / 2) - aLevel.Player.Vision);
			lPlayer.y = aLevel.Player.Vision + this.MapPosY + ((this.MapHeight / 2) - aLevel.Player.Vision);

			//print character
			printChar (tPlayer, 
				lPlayer.x ,
				lPlayer.y);

			if (aLevel.Player.Mode == PlayerMode.Interact) {
				//print selection if it exists
				printChar (tSelection, 
					aLevel.Player.Selection.x + lPlayer.x ,
					aLevel.Player.Selection.y + lPlayer.y);
			}
		}

		public void printStats(Level aLevel){
			printLine ("Name:",this.StatWidth,this.StatPosX,this.StatPosY+1);
			printLine (aLevel.Player.Name,this.StatWidth,this.StatPosX,this.StatPosY+2);
			printLine ("Health:",this.StatWidth,this.StatPosX,this.StatPosY+4);
			printLine (aLevel.Player.Health.ToString(),this.StatWidth,this.StatPosX,this.StatPosY+5);
			printLine ("Str:",this.StatWidth,this.StatPosX,this.StatPosY+7);
			printLine (aLevel.Player.Strength.ToString(),this.StatWidth,this.StatPosX,this.StatPosY+8);

			//print Inventory
			printLine ("Inventory:",this.StatWidth,this.StatPosX + (this.StatWidth / 2),this.StatPosY+1);
			for(int i = 0; i < aLevel.Player.Inventory.Count && i < this.StatWidth - 3; i++){
				printLine (aLevel.Player.Inventory[i].GetName(),this.StatWidth,this.StatPosX + (this.StatWidth / 2),this.StatPosY + 2 + i);
			}
		}

		public void printHistory(Level aLevel){
			clearArea (this.HistPosX, this.HistPosY, this.HistWidth, this.HistHeight);
			for (int y = 0; (y < this.HistHeight) && (y < aLevel.History.HistoryList.Count); y++) {
				if (aLevel.History.HistoryList.Count <= this.HistHeight) {
					printLine (aLevel.History.HistoryList [y], this.HistWidth, this.HistPosX, this.HistPosY + y);
				} else {
					printLine (aLevel.History.HistoryList [aLevel.History.HistoryList.Count - this.HistHeight + y], 
						this.HistWidth, this.HistPosX, this.HistPosY + y);
				}
			}
		}

		public void printTextBox(String aText, int aX, int aY, int aWidth, int aHeight){
			printBorder (aX, aY, aWidth, aHeight);
			clearArea (aX+1,aY+1,aWidth,aHeight);
			if (aText.Length > aWidth) {
				for (int y=aY;y<aY+aHeight;y++){
					if ((y * aWidth) > aText.Length) {
						break;
					}
					for (int x=aX;x<aX+aWidth;x++){
						if (((y * aWidth) + x) > aText.Length) {
							break;
						}
						printChar (aText[(y*aWidth)+x],x+1,y+1);
					}
				}

			} else {
				printLine(aText,aWidth,aX+1,aY+1);
			}
		}

		private void printBorder(int aX, int aY, int aWidth, int aHeight){
			int lXMax = aX + aWidth - 1;
			int lYMax = aY + aHeight - 1;

			for (int y=aY;y<lYMax;y++){
				printChar (tBorderV,aX, y);
				printChar (tBorderV,lXMax, y);
			}
			for (int x=aX;x<lXMax;x++){
				printChar (tBorderH,x, aY);
				printChar (tBorderH,x, lYMax);
			}
			printChar (tBorderC,aX, aY);
			printChar (tBorderC,aX, lYMax);
			printChar (tBorderC,lXMax, aY);
			printChar (tBorderC,lXMax, lYMax);
		}
			

		private void printChar(char aChar, int aX, int aY){
			if((aX >=0 && aX < this.ScreenWidth) &&
				(aY >= 0 && aY < this.ScreenHeight)){
				this.Screen [aX, aY] = aChar;
			}
		}

		private void printLine(String aString,int aLength, int aX, int aY){
			for (int x=0;(x < aString.Length) && (x < aLength);x ++){
				printChar (aString [x], aX+x, aY);
			}
		}

		private void clearArea(int aX, int aY, int aWidth, int aHeight){
			for (int y=aY; y < (aY + aHeight); y++){
				for (int x=aX; x < (aX + aWidth); x++){
					printChar (' ',x,y);
				}
			}
		}
	}
}


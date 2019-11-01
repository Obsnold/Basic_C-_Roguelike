using System;
using System.Collections.Generic;

namespace RogueLike
{
	public enum LevelTiles{
		Wall,
		Floor,
		DoorClosed,
		DoorOpen,
		Ladder,
	}

	public struct room{
		public int x;
		public int y;
		public int h;
		public int w;
	}

	public class Level
	{
		string Tag;
		public int Width;
		public int Height;
		public bool[,] VisibilityGrid;
		public LevelTiles [,] TileGrid;
		//public ObjectInterface[,] 
		public Creature[,] CreatureGrid;
		public Creature Player;
		public List<room> RoomList;
		public History History;
		Debug Debug = Debug.Instance;

		public Level (int aWidth, int aHeight)
		{
			this.Tag = this.GetType().Name;
			this.Width = aWidth;
			this.Height = aHeight;
			this.Player = new Creature("Player", this.Width / 2, this.Height/2, 20 ,2,0);
			this.TileGrid = new LevelTiles[this.Width,this.Height];
			this.CreatureGrid = new Creature[this.Width,this.Height];
			this.VisibilityGrid = new bool[this.Width, this.Height];
			this.RoomList = new List<room>();
			this.History = new History ();
		}

		public bool moveCreature(Creature aCreature, int aX, int aY){
			bool result = false;

			if((aX < this.Width) && (aY < this.Height) &&
				(aX > 0) && (aY > 0) &&
				((this.TileGrid[aCreature.X,aCreature.Y] == LevelTiles.Ladder) ||
					(this.TileGrid[aCreature.X,aCreature.Y] == LevelTiles.Floor) ||
					(this.TileGrid[aCreature.X,aCreature.Y] == LevelTiles.DoorOpen)) &&
				(this.CreatureGrid[aX,aY] == null))
			{
				this.CreatureGrid [aCreature.X, aCreature.Y] = null;
				aCreature.X = aX;
				aCreature.Y = aY;
				this.CreatureGrid [aCreature.X, aCreature.Y] = aCreature;
				result = true;
			}
			return result;
		}

		public List<Creature> getCreatureList(){
			List<Creature> lTempList = new List<Creature> ();

			for (int y = 0; y < this.Height; y++) {
				for (int x = 0; x < this.Width; x++) {
					if(this.CreatureGrid[x,y] != null){
						lTempList.Add (this.CreatureGrid [x, y]);
					}
				}
			}
			return lTempList;
		}

		public Creature getCreature(int aX, int aY){
			if(this.CreatureGrid[aX,aY] != null){
				return this.CreatureGrid [aX, aY];
			} 
			return null;
		}

		public void removeCreature(Creature aCreature){
			this.CreatureGrid [aCreature.X, aCreature.Y] = null;
		}

		public void ComputePlayerFOV(){
			for(int y = 0; y < this.Height; y++){
				for (int x = 0; x < this.Width; x++) {
					this.VisibilityGrid [x, y] = false;
				}
			}
			int lY = CheckY(this.Player.Y - this.Player.Vision);
			int lX = CheckX(this.Player.X - this.Player.Vision);

			for (int y = lY; y < (this.Player.Vision*2)+1+lY; y++) {
				for (int x = lX; x < (this.Player.Vision*2)+1+lX; x++) {
					this.VisibilityGrid[CheckX(x),CheckY(y)] = InLineOfSight (this.Player,x,y);
				}
			}

		}

		public int CheckX(int aX){
			int lX = aX;
			if(lX < 0){
				lX = 0;
			}
			if(lX >= this.Width){
				lX = this.Width-1;
			}
			return lX;
		}

		public int CheckY(int aY){
			int lY = aY;
			if(lY < 0){
				lY = 0;
			}
			if(lY >= this.Height){
				lY = this.Height-1;
			}
			return lY;
		}

		public bool InLineOfSight(Creature aCreature, Creature aTarget){
			return InLineOfSight (aCreature, aTarget.X, aTarget.Y);
		}

		public bool InLineOfSight(Creature aCreature, int aX, int aY){
			bool lCanSee = true;
			int lX1 = aCreature.X;
			int lY1 = aCreature.Y;
			int lX2 = aX;
			int lY2 = aY;

			int lDeltaX = lX2 - lX1;
			int lIX = lDeltaX > 0 ? 1 : -1;
			lDeltaX = Math.Abs (lDeltaX) * 2;

			int lDeltaY = lY2 - lY1;
			int lIY = lDeltaY > 0 ? 1 : -1;
			lDeltaY = Math.Abs (lDeltaY) * 2;


			if (lDeltaY >= lDeltaX) {
				int lError = lDeltaY - (lDeltaX / 2);
				while (lX1 != lX2) {
					if (SightBlocked (lX1, lY1)) {
						lCanSee = false;
						break;
					}
					if ((lError > 0) || ((lError == 0) && (lIX > 0))) {
						lError -= lDeltaX;
						lY1 += lIY;
					}
					lError += lDeltaY;
					lX1 += lIX;
				}
				while (lY1 != lY2) {
					if (SightBlocked (lX1, lY1)) {
						lCanSee = false;
						break;
					}
					lY1 += lIY;
				}

			} else {
				int lError = lDeltaX - (lDeltaY / 2);
				while (lY1 != lY2)  {
					if (SightBlocked (lX1, lY1)) {
						lCanSee = false;
						break;
					}
					if ((lError > 0) || ((lError == 0) && (lIY > 0))) {
						lError -= lDeltaY;
						lX1 += lIX;
					}
					lError += lDeltaX;
					lY1 += lIY;
				}
				while (lX1 != lX2) {
					if (SightBlocked (lX1, lY1)) {
						lCanSee = false;
						break;
					}
					lX1 += lIX;
				}
			}


			this.Debug.Print (this.Tag, "InLineOfSight: " + lCanSee.ToString () + " playerX:" + aCreature.X.ToString () 
				+ " playerY:" + aCreature.Y.ToString () + " lX1:" + lX1.ToString () + " lY1:" + lY1.ToString (), 20);

			return lCanSee;
		}

		public bool SightBlocked(int aX, int aY){
			bool lIsBlocked = false;

			if((this.TileGrid[aX,aY] == LevelTiles.DoorClosed) || 
				(this.TileGrid[aX,aY] == LevelTiles.Wall)){
				lIsBlocked = true;
			}
			return lIsBlocked;
		}
	}
}


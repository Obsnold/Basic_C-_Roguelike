using System;
using System.Collections.Generic;

namespace RogueLike
{
	public enum LevelTiles{
		Wall,
		Floor,
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


		public Grid<LevelTiles> BaseGrid;
		public Grid<ObjectInterface> ObjectGrid;
		public Grid<Creature> CreatureGrid;
		public Grid<bool> VisibilityGrid;

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
			this.BaseGrid = new Grid<LevelTiles>(this.Width,this.Height);
			this.ObjectGrid = new Grid<ObjectInterface>(this.Width,this.Height);
			this.CreatureGrid = new Grid<Creature>(this.Width,this.Height);
			this.VisibilityGrid = new Grid<bool>(this.Width,this.Height);
			this.RoomList = new List<room>();
			this.History = new History ();
		}

		public bool moveCreature(Creature aCreature, int aX, int aY){
			bool result = false;
			if(this.InBounds(aX,aY) &&
				(!this.PathBlocked(aX,aY)))
			{
				this.CreatureGrid.SetItem (null, aCreature.pos.x, aCreature.pos.y);
				aCreature.pos.x = aX;
				aCreature.pos.y = aY;
				this.CreatureGrid.SetItem (aCreature, aCreature.pos.x, aCreature.pos.y);
				result = true;
			}
			return result;
		}

		public bool moveCreature(Creature aCreature, Coodinate aCoord){
			return moveCreature(aCreature, aCoord.x, aCoord.y);
		}

		public List<Creature> getCreatureList(){
			List<Creature> lTempList = new List<Creature> ();

			for (int y = 0; y < this.Height; y++) {
				for (int x = 0; x < this.Width; x++) {
					Creature lTemp = this.CreatureGrid.GetItem (x, y);
					if(lTemp != null){
						lTempList.Add (lTemp);
					}
				}
			}
			return lTempList;
		}

		public Creature getCreature(int aX, int aY){
			return this.CreatureGrid.GetItem (aX, aY);
		}

		public void removeCreature(Creature aCreature){
			this.CreatureGrid.SetItem(null, aCreature.pos.x, aCreature.pos.y);
		}

		public void ComputePlayerFOV(){
			for(int y = 0; y < this.Height; y++){
				for (int x = 0; x < this.Width; x++) {
					this.VisibilityGrid.SetItem(false,x, y);
				}
			}
			int lY = CheckY(this.Player.pos.y - this.Player.Vision);
			int lX = CheckX(this.Player.pos.x - this.Player.Vision);

			for (int y = lY; y < (this.Player.Vision*2)+1+lY; y++) {
				for (int x = lX; x < (this.Player.Vision*2)+1+lX; x++) {
					this.VisibilityGrid.SetItem (InLineOfSight (this.Player,x,y),CheckX (x), CheckY (y));
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

		public bool InBounds(int aX, int aY){
			bool lInBounds = true;
			if((aX < 0) || (aX >= this.Width) ||
				(aY < 0) || (aY >= this.Height)){
				lInBounds = false;
			}
			return lInBounds;
		}

		public bool InLineOfSight(Creature aCreature, Creature aTarget){
			return InLineOfSight (aCreature, aTarget.pos.x, aTarget.pos.y);
		}

		public bool InLineOfSight(Creature aCreature, Coodinate aCoord){
			return InLineOfSight (aCreature, aCoord.x, aCoord.y);
		}

		public bool InLineOfSight(Creature aCreature, int aX, int aY){
			bool lCanSee = true;
			int lX1 = aCreature.pos.x;
			int lY1 = aCreature.pos.y;
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


			this.Debug.Print (this.Tag, "InLineOfSight: " + lCanSee.ToString () + " playerX:" + aCreature.pos.x.ToString () 
				+ " playerY:" + aCreature.pos.y.ToString () + " lX1:" + lX1.ToString () + " lY1:" + lY1.ToString (), 20);

			return lCanSee;
		}

		public bool SightBlocked(int aX, int aY){
			bool lIsBlocked = false;

			//check base grid
			if((this.BaseGrid.GetItem(aX,aY) == LevelTiles.Wall)){
				lIsBlocked = true;
			}

			//check object grid
			ObjectInterface lObj = ObjectGrid.GetItem(aX,aY);
			if (lObj != null) {
				if(!lObj.CanSeePast()){
					lIsBlocked = true;
				}
			}
			return lIsBlocked;
		}

		public bool PathBlocked(int aX, int aY){
			bool lIsBlocked = false;

			//check base grid
			if((this.BaseGrid.GetItem(aX,aY) == LevelTiles.Wall)){
				lIsBlocked = true;
			}

			//check object grid
			ObjectInterface lObj = ObjectGrid.GetItem(aX,aY);
			if (lObj != null) {
				if(!lObj.CanWalk()){
					lIsBlocked = true;
				}
			}

			//check Creaturegrid
			if (CreatureGrid.GetItem(aX,aY) != null) {
				lIsBlocked = true;
			}

			return lIsBlocked;

		}
	}
}


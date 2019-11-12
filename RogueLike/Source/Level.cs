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
		public Grid<Actor> ActorGrid;
		public Grid<Item> ItemGrid;
		public Grid<bool> VisibilityGrid;

		public Player Player;

		public List<room> RoomList;
		public History History;
		Debug Debug = Debug.Instance;

		public Level (int aWidth, int aHeight)
		{
			this.Tag = this.GetType().Name;
			this.Width = aWidth;
			this.Height = aHeight;

			this.BaseGrid = new Grid<LevelTiles>(this.Width,this.Height);
			this.ObjectGrid = new Grid<ObjectInterface>(this.Width,this.Height);
			this.ActorGrid = new Grid<Actor>(this.Width,this.Height);
			this.ItemGrid = new Grid<Item>(this.Width,this.Height);
			this.VisibilityGrid = new Grid<bool>(this.Width,this.Height);
			this.RoomList = new List<room>();
			this.History = new History ();
		}

		public List<Actor> getActorList(){
			List<Actor> lTempList = new List<Actor> ();

			for (int y = 0; y < this.Height; y++) {
				for (int x = 0; x < this.Width; x++) {
					Actor lTemp = this.ActorGrid.GetItem (x, y);
					if(lTemp != null){
						lTempList.Add (lTemp);
					}
				}
			}
			return lTempList;
		}

		public Actor getActor(int aX, int aY){
			return this.ActorGrid.GetItem (aX, aY);
		}

		public void removeActor(Actor aActor){
			this.ActorGrid.SetItem(null, aActor.GetPos());
		}

		//TODO: very inefficient should be rewritten in the future
		public void ComputePlayerFOV(){
			for(int y = 0; y < this.Height; y++){
				for (int x = 0; x < this.Width; x++) {
					this.VisibilityGrid.SetItem(false,x, y);
				}
			}
			int lY = CheckY(this.Player.GetPos().y - this.Player.Vision);
			int lX = CheckX(this.Player.GetPos().x - this.Player.Vision);

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

		public bool InLineOfSight(Actor aActor, Actor aTarget){
			return InLineOfSight (aActor, aTarget.GetPos().x, aTarget.GetPos().y);
		}

		public bool InLineOfSight(Actor aActor, Coordinate aCoord){
			return InLineOfSight (aActor, aCoord.x, aCoord.y);
		}


		public bool InLineOfSight(Actor aActor, int aX, int aY, List<Coordinate> aCoords = null){
			bool lCanSee = true;
			Coordinate lCoord1;
			Coordinate lCoord2;
			Coordinate lDelta;
			Coordinate lI;
			lCoord1 = aActor.GetPos();
			lCoord2.x = aX;
			lCoord2.y = aY;


			lDelta.x = lCoord2.x - lCoord1.x;
			lI.x = lDelta.x > 0 ? 1 : -1;
			lDelta.x = Math.Abs (lDelta.x) * 2;

			lDelta.y = lCoord2.y - lCoord1.y;
			lI.y = lDelta.y > 0 ? 1 : -1;
			lDelta.y = Math.Abs (lDelta.y) * 2;


			if (lDelta.y >= lDelta.x) {
				int lError = lDelta.y - (lDelta.x / 2);
				while (lCoord1.x != lCoord2.x) {
					if (SightBlocked (lCoord1.x, lCoord1.y)) {
						lCanSee = false;
						break;
					}
					if ((lError > 0) || ((lError == 0) && (lI.x > 0))) {
						lError -= lDelta.x;
						lCoord1.y += lI.y;
					}
					lError += lDelta.y;
					lCoord1.x += lI.x;
					if (aCoords != null) {
						aCoords.Add (lCoord1);
					}
				}
				while (lCoord1.y != lCoord2.y) {
					if (SightBlocked (lCoord1.x, lCoord1.y)) {
						lCanSee = false;
						break;
					}
					lCoord1.y += lI.y;
				}

			} else {
				int lError = lDelta.x - (lDelta.y / 2);
				while (lCoord1.y != lCoord2.y)  {
					if (SightBlocked (lCoord1.x, lCoord1.y)) {
						lCanSee = false;
						break;
					}
					if ((lError > 0) || ((lError == 0) && (lI.y > 0))) {
						lError -= lDelta.y;
						lCoord1.x += lI.x;
					}
					lError += lDelta.x;
					lCoord1.y += lI.y;
					if (aCoords != null) {
						aCoords.Add (lCoord1);
					}
				}
				while (lCoord1.x != lCoord2.x) {
					if (SightBlocked (lCoord1.x, lCoord1.y)) {
						lCanSee = false;
						break;
					}
					lCoord1.x += lI.x;
				}
			}


			this.Debug.Print (this.Tag, "InLineOfSight: " + lCanSee.ToString () + " playerX:" + aActor.GetPos().x.ToString () 
				+ " playerY:" + aActor.GetPos().y.ToString () + " lX1:" + lCoord1.x.ToString () + " lY1:" + lCoord1.x.ToString (), 20);

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

			//check Actorgrid
			if (ActorGrid.GetItem(aX,aY) != null) {
				lIsBlocked = true;
			}

			return lIsBlocked;

		}
	}
}


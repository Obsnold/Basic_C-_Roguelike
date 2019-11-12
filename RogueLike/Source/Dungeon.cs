using System;
using System.Collections.Generic;

namespace RogueLike
{
	public class Dungeon
	{
		static Dungeon mDungeon;
		LevelGenerator LevelGen;
		List<Level> Floors;
		int FloorWidth = 69;
		int FloorHeight = 18;
		int CurrentFloor = 0;



		public Dungeon ()
		{
			
		}

		public static Dungeon Instance
		{
			get{
				if (mDungeon == null) {
					mDungeon = new Dungeon ();
				} 
				return mDungeon;
			}
		}

		public void Initialise(){
			this.LevelGen = new LevelGenerator (FloorWidth,FloorHeight);
			this.LevelGen.genLevel ();
			this.Floors = new List<Level>();
			this.Floors.Add(this.LevelGen.getLevel ());

			this.LevelGen.genActors (this.Floors [CurrentFloor]);
			this.LevelGen.genItems (this.Floors [CurrentFloor]);
			this.Floors[CurrentFloor].Player.SetPos(this.Floors[CurrentFloor].Player.pos);
		}

		public Level GetCurrentLevel(){
			return this.Floors [CurrentFloor];
		}
	}
}


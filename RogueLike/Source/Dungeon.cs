using System;
using System.Collections.Generic;

namespace RogueLike
{
	public class Dungeon
	{
		static Dungeon mDungeon;
		LevelGenerator LevelGen;
		ActorGenerator ActorGen;
		ItemGenerator ItemGen;
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
			this.ActorGen = new ActorGenerator ();
			this.ItemGen = new ItemGenerator ();
			this.LevelGen.genLevel ();
			this.Floors = new List<Level>();
			this.Floors.Add(this.LevelGen.getLevel ());

			this.ActorGen.PopulateLevel (this.Floors [CurrentFloor]);
			this.ItemGen.PopulateLevel (this.Floors [CurrentFloor]);
			this.Floors[CurrentFloor].Player.SetPos(this.Floors[CurrentFloor].Player.GetPos());
		}

		public Level GetCurrentLevel(){
			return this.Floors [CurrentFloor];
		}
	}
}


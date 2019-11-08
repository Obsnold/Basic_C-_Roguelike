using System;
using System.Collections.Generic;

namespace RogueLike
{

	public class GameLogic
	{
		LevelGenerator LevelGen;
		Display Display;
		Level Level;
		int floorWidth = 69;
		int floorHeight = 18;
		Debug debug =  Debug.Instance;

		public GameLogic (Display aDisplay)
		{
			this.LevelGen = new LevelGenerator (floorWidth,floorHeight);
			this.Display = aDisplay;
			this.LevelGen.genLevel ();
			this.Level = this.LevelGen.getLevel ();
			this.Level.moveActor (this.Level.Player,this.Level.Player.pos.x,this.Level.Player.pos.y);
			this.Level.ComputePlayerFOV ();
			this.Display.printMainScreen (this.Level);
		}

		public bool GameTick(){
			bool returnValue = true;

			//work out AI actions
			List<Actor> lActorList = this.Level.getActorList ();
			if (lActorList != null && lActorList.Count > 0) {
				foreach (Actor lActor in lActorList) {
					if (!lActor.IsDead ()) {
						Action lAction = lActor.TakeTurn ();
						while (true) {
							ActionResult lResult = lAction.Perform ();
							if (lResult.Result) {
								break;
							} else if (lResult.AlternateAction == null) {
								break;
							}
							lAction = lResult.AlternateAction;
						}
						this.Level.ComputePlayerFOV ();
						this.Display.printMainScreen (this.Level);
					}
					if (this.Level.Player.IsDead ()) {
						returnValue = false;
						break;
					}
				}
			}

			return returnValue;
		}
	}
}


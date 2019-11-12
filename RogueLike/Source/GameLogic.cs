using System;
using System.Collections.Generic;

namespace RogueLike
{

	public class GameLogic
	{
		Display Display = Display.Instance;
		Dungeon Dungeon = Dungeon.Instance;
		Debug debug =  Debug.Instance;
		Level Level;

		public GameLogic ()
		{
			this.Dungeon.Initialise ();
			this.Level = Dungeon.GetCurrentLevel ();
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


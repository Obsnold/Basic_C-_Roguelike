using System;

namespace RogueLike
{
	public class MoveAction : Action
	{
		Level Level;
		Actor Actor;
		Direction Dir;
		Debug debug =  Debug.Instance;

		public MoveAction (Actor aActor, Direction aDir)
		{
			Dungeon lDungeon = Dungeon.Instance;
			this.Level = lDungeon.GetCurrentLevel();
			this.Actor = aActor;
			this.Dir = aDir;
		}

		public override ActionResult Perform(){
			ActionResult lResult = new ActionResult(false);
			Coordinate lNewPos = this.Actor.pos + this.Dir;

			// check if there is anyone in the space
			Actor lTarget = this.Level.ActorGrid.GetItem(lNewPos);
			if (lTarget != null) {
				lResult.Alternate (new AttackAction(this.Actor,lTarget));
				return lResult;
			}

			//is it a door or something that can be opened
			ObjectInterface lObject = this.Level.ObjectGrid.GetItem(lNewPos);
			if (lObject != null) {
				if (!lObject.CanWalk ()) {
					lResult.Alternate(lObject.DefaultAction (this.Actor));
					return lResult;
				}
			}

			//set new position
			this.Actor.SetPos(lNewPos);
			lResult.Success ();
			debug.Print ("MoveAction: ","Name:" + this.Actor.Name.ToString() +" pos:" + lNewPos.ToString() + " dir:" + this.Dir.ToString(),20);

			ItemInterface lItem = this.Level.ItemGrid.GetItem(lNewPos);
			if (lItem != null) {
				lResult.Success ("There is a " + lItem.GetDescription () + " on the floor here.");
			} else {
				lResult.Success ();
			}
				
			return lResult;
		}
	}
}


using System;

namespace RogueLike
{
	public class OpenAction : Action
	{
		ObjectInterface Object;
		Actor Actor;
		Level Level;

		public OpenAction (Level aLevel,Actor aActor, ObjectInterface aObject)
		{
			this.Level = aLevel;
			this.Actor = aActor;
			this.Object = aObject;
		}

		public override ActionResult Perform (){
			ActionResult lResult = new ActionResult(true);
			this.Object.Interact (this.Actor);
			return lResult;
		}
	}
}


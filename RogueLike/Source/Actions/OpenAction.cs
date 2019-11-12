using System;

namespace RogueLike
{
	public class OpenAction : Action
	{
		ObjectInterface Object;
		Actor Actor;

		public OpenAction (Actor aActor, ObjectInterface aObject)
		{
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


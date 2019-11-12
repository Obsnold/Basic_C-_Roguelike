using System;

namespace RogueLike
{
	public class CloseAction : Action
	{
		ObjectInterface Object;
		Actor Actor;

		public CloseAction (Actor aActor, ObjectInterface aObject)
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


using System;

namespace RogueLike
{
	public class ConsumeAction : Action
	{
		ItemInterface Item;
		Actor Actor;

		public ConsumeAction (Actor aActor, ItemInterface aItem)
		{
			this.Actor = aActor;
			this.Item = aItem;
		}

		public override ActionResult Perform (){
			ActionResult lResult = new ActionResult(true);
			this.Item.Interact (this.Actor);
			this.Actor.Inventory.Remove (this.Item);
			return lResult;
		}
	}
}


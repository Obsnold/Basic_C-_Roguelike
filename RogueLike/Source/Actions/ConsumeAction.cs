using System;

namespace RogueLike
{
	public class ConsumeAction : Action
	{
		ItemInterface Item;
		Actor Actor;
		Level Level;

		public ConsumeAction (Level aLevel,Actor aActor, ItemInterface aItem)
		{
			this.Level = aLevel;
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


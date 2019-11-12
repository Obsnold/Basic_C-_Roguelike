using System;

namespace RogueLike
{
	public class ConsumeAction : Action
	{
		Item Item;
		Actor Actor;

		public ConsumeAction (Actor aActor, Item aItem)
		{
			this.Actor = aActor;
			this.Item = aItem;
		}

		public override ActionResult Perform (){
			ActionResult lResult = new ActionResult(true);
			this.Item.use.use(this.Actor);
			this.Actor.Inventory.Remove (this.Item);
			return lResult;
		}
	}
}


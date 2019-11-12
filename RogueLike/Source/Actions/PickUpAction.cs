using System;

namespace RogueLike
{
	public class PickUpAction : Action
	{
		Item Item;
		Actor Actor;
		Level Level;

		public PickUpAction (Actor aActor, Item aItem)
		{
			Dungeon lDungeon = Dungeon.Instance;
			this.Level = lDungeon.GetCurrentLevel();
			this.Actor = aActor;
			this.Item = aItem;
		}

		public override ActionResult Perform (){
			ActionResult lResult = new ActionResult(true);
			this.Actor.Inventory.Add (this.Item);
			this.Level.ItemGrid.SetItem (null, this.Item.GetPos ());
			return lResult;
		}
	}
}


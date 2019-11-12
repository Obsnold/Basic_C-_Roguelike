using System;

namespace RogueLike
{
	public class PickUpAction : Action
	{
		ItemInterface Item;
		Actor Actor;
		Level Level;

		public PickUpAction (Actor aActor, ItemInterface aItem)
		{
			Dungeon lDungeon = Dungeon.Instance;
			this.Level = lDungeon.GetCurrentLevel();
			this.Actor = aActor;
			this.Item = aItem;
		}

		public override ActionResult Perform (){
			ActionResult lResult = new ActionResult(true);
			this.Actor.Inventory.Add (this.Item);
			this.Level.ItemGrid.SetItem (null, this.Item.GetPosition ());
			return lResult;
		}
	}
}


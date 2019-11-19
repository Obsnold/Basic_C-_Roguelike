using System;

namespace RogueLike
{
	public enum PlayerMode{
		Normal,
		Interact,
		Inventory
	}

	public class Player : Actor
	{
		public PlayerMode Mode = PlayerMode.Normal;
		public Coordinate Selection;
		public int InvSelection;
		Display Display = Display.Instance;

		public Player (String aName, ActorTemplate aTemplate, int aX, int aY, int aGroup):
		base (aName, aTemplate, aX, aY, aGroup)
		{
		}

		public override Action TakeTurn (){
			Action lAction = null;
			//get input
			do {
				RogueKey lKeyPressed = Input.getKey (Console.ReadKey (true).Key);
				switch (Mode) {
				case PlayerMode.Normal:
					lAction = NormalMode (lKeyPressed);
					break;
				case PlayerMode.Interact:
					lAction = InteractMode (lKeyPressed);
					break;
				case PlayerMode.Inventory:
					lAction = InventoryMode (lKeyPressed);
					break;
				}
				base.level.ComputePlayerFOV ();
				Display.printScreen (base.level, this.Mode);

			} while(lAction == null);

			return lAction;
		}


		Action NormalMode(RogueKey aKeyPress){
			Action lAction = null;

			if (Input.IsDirectionKey (aKeyPress)) {
				//deal with direction key press
				lAction = new MoveAction(this,Input.DirectionKeyToDirection(aKeyPress));
			}else {
				switch (aKeyPress){
				case RogueKey.ChangeMode:
					Mode = PlayerMode.Interact;
					break;
				case RogueKey.Cancel:
					break;
				
				}

			}
			return lAction;
		}

		Action InteractMode(RogueKey aKeyPress){
			Action lAction = null;

			if (Input.IsDirectionKey (aKeyPress)) {
				this.Selection = Input.DirectionKeyToCoordinate (aKeyPress);
			} else {
				switch (aKeyPress){
				case RogueKey.ChangeMode:
					Mode = PlayerMode.Inventory;
					this.Selection.x = 0;
					this.Selection.y = 0;
					break;
				case RogueKey.Cancel:
					Mode = PlayerMode.Normal;
					this.Selection.x = 0;
					this.Selection.y = 0;
					break;
				case RogueKey.Select:
					if (this.level.ActorGrid.GetItem (this.pos + this.Selection) != null) {
						lAction = new AttackAction (this, this.level.ActorGrid.GetItem (this.pos + this.Selection));
					} else if (this.level.ObjectGrid.GetItem(this.pos + this.Selection) != null){
						lAction = this.level.ObjectGrid.GetItem (this.pos + this.Selection).DefaultAction (this);
					} else if (this.level.ItemGrid.GetItem(this.pos + this.Selection) != null){
						lAction = new PickUpAction (this, this.level.ItemGrid.GetItem (this.pos + this.Selection));
					}
					Mode = PlayerMode.Normal;
					this.Selection.x = 0;
					this.Selection.y = 0;
					break;
				}
			}
			return lAction;
		}

		Action InventoryMode(RogueKey aKeyPress){
			Action lAction = null;

			switch (aKeyPress){
			case RogueKey.ChangeMode:
				Mode = PlayerMode.Normal;
				this.Selection.x = 0;
				this.Selection.y = 0;
				break;
			case RogueKey.Cancel:
				Mode = PlayerMode.Normal;
				this.Selection.x = 0;
				this.Selection.y = 0;
				break;
			case RogueKey.North:
				if (this.Inventory != null && this.Inventory.Count > 0 && InvSelection > 0) {
					InvSelection--;
				}
				break;
			case RogueKey.South:
				if (this.Inventory != null && InvSelection < (this.Inventory.Count - 1)) {
					InvSelection++;
				}
				break;
			case RogueKey.Select:
				if (this.Inventory != null && this.Inventory.Count >= 0 && InvSelection < this.Inventory.Count){
					Item lItem = this.Inventory [InvSelection];
					bool lResult = true;
					if (lItem.EquipTo == null) {
						lAction = new ConsumeAction (this, lItem);
					} else {
						lResult = base.Equip (lItem);
					}
					if (lResult) {
						if (InvSelection > 0) {
							InvSelection--;
						} else {
							InvSelection = 0;
						}
					}
				}
				break;
			}

			return lAction;
		}
	}
}


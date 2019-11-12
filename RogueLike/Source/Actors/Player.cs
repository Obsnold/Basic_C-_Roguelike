using System;

namespace RogueLike
{
	public enum PlayerMode{
		Normal,
		Interact
	}

	public class Player : Actor
	{
		public PlayerMode Mode = PlayerMode.Normal;
		public Coordinate Selection;
		Display Display = Display.Instance;

		public Player (String aName, int aX, int aY, int aHealth ,int aStrength, int aGroup):
		base (aName, aX, aY, aHealth , aStrength, aGroup)
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
				}
				base.level.ComputePlayerFOV ();
				this.Display.printMainScreen (base.level);
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
				case RogueKey.Select:
					if (this.Inventory != null && this.Inventory.Count > 0){
						lAction = new ConsumeAction (this, this.Inventory [0]);
					}
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
					Mode = PlayerMode.Normal;
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
	}
}


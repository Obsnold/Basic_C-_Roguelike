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

		public Player (String aName, int aX, int aY, int aHealth ,int aStrength, int aGroup, Level aLevel):
		base (aName, aX, aY, aHealth , aStrength, aGroup, aLevel)
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
			} while(lAction == null);

			return lAction;
		}


		Action NormalMode(RogueKey aKeyPress){
			Action lAction = null;

			if (Input.IsDirectionKey (aKeyPress)) {
				//deal with direction key press
				lAction = new MoveAction(this.Level,this,Input.DirectionKeyToDirection(aKeyPress));
			}else {
				switch (aKeyPress){
				case RogueKey.ChangeMode:
					Mode = PlayerMode.Interact;
					break;
				case RogueKey.Cancel:
					break;
				case RogueKey.Select:
					//temporarily use this to use an item
					//returnAction.Type = ActionType.
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
					break;
				case RogueKey.Select:
					if (this.Level.ActorGrid.GetItem (this.pos + this.Selection) != null) {
						lAction = new AttackAction (this.Level, this, this.Level.ActorGrid.GetItem (this.pos + this.Selection));
					} else if (this.Level.ObjectGrid.GetItem(this.pos + this.Selection) != null){
						lAction = this.Level.ObjectGrid.GetItem (this.pos + this.Selection).DefaultAction (this.Level, this);
					} else if (this.Level.ItemGrid.GetItem(this.pos + this.Selection) != null){
						//implement later
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


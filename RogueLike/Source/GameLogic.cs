using System;
using System.Collections.Generic;

namespace RogueLike
{
	public enum GameMode{
		Normal,
		Interact,
		SystemMenu,
		CharacterMenu
	}

	public class GameLogic
	{
		string Tag;
		Debug Debug = Debug.Instance;
		LevelGenerator LevelGen;
		Display Display;
		Level Level;
		Random Rand;
		int floorWidth = 69;
		int floorHeight = 18;
		GameMode Mode = GameMode.Normal;
		Coordinate Selection;

		public GameLogic (Display aDisplay)
		{
			this.LevelGen = new LevelGenerator (floorWidth,floorHeight);
			this.Display = aDisplay;
			this.Rand = new Random ();
			this.LevelGen.genLevel ();
			this.Level = this.LevelGen.getLevel ();
			this.Level.moveCreature (this.Level.Player,this.Level.Player.pos.x,this.Level.Player.pos.y);
			this.Level.ComputePlayerFOV ();
			this.Display.printMainScreen (this.Level);
			this.Tag = this.GetType().Name;
			this.Selection.x = 0;
			this.Selection.y = 0;

		}

		public bool GameTick(RogueKey aKeyPress){
			bool returnValue = true;
			Action lAction = new Action (ActionType.None, 0, 0);

			//work out player action if any
			switch (Mode){
			case GameMode.Normal:
				lAction = NormalMode (aKeyPress);
				break;
			case GameMode.Interact:
				lAction = InteractMode (aKeyPress);
				break;
			}
			if (lAction != null) {
				//has player taken an action?
				if (ProcessAction (this.Level.Player, lAction)) {
					//work out AI actions
					List<Creature> lCreatureList = this.Level.getCreatureList ();
					lCreatureList.Remove (this.Level.Player);
					if (lCreatureList != null && lCreatureList.Count > 0) {
						foreach (Creature lCreature in lCreatureList) {
							if (!lCreature.IsDead ()) {
								lAction = AICreatureAction (lCreature);
								ProcessAction (lCreature, lAction);
							}
							if (this.Level.Player.IsDead ()) {
								returnValue = false;
								break;
							}
						}
					}
				}

				// Display screen
				this.Level.ComputePlayerFOV ();
				switch (Mode) {
				case GameMode.Normal:
					this.Display.printMainScreen (this.Level);
					break;
				case GameMode.Interact:
					this.Display.printMainScreen (this.Level, this.Selection);
					break;
				}
			} else {
				returnValue = false;
			}

			return returnValue;
		}

		Action NormalMode(RogueKey aKeyPress){
			Action returnAction = new Action (ActionType.None, this.Level.Player.pos.x, this.Level.Player.pos.y);

			this.Debug.Print  ( this.Tag, "KEYPRESS:" + aKeyPress.ToString (),20);
			if (Input.IsDirectionKey (aKeyPress)) {
				//deal with direction key press
				returnAction.pos += Input.DirectionKeyToCoordinate (aKeyPress);					
				if (this.Level.CreatureGrid.GetItem (returnAction.pos) != null) {
					//If there is a creature
					Creature lTarget = this.Level.CreatureGrid.GetItem (returnAction.pos);
					if (lTarget.Group != this.Level.Player.Group) {
						returnAction.Type = ActionType.Attack;
					}
				} else if (this.Level.ObjectGrid.GetItem (returnAction.pos) != null) {
					//if there is an object
					ObjectInterface lObject = this.Level.ObjectGrid.GetItem (returnAction.pos);
					if (lObject.CanWalk ()) {
						this.Debug.Print (this.Tag, "DOOR_MOVE", 20);
						returnAction.Type = ActionType.Move;
					} else {
						this.Debug.Print (this.Tag, "DOOR_INTERACT", 20);
						returnAction.Type = ActionType.Interact;
					}
				} else {
					//else just work it out based on base grid
					switch (this.Level.BaseGrid.GetItem (returnAction.pos)) {
					case LevelTiles.Floor:
						this.Debug.Print (this.Tag, "FLOOR_MOVE", 20);
						returnAction.Type = ActionType.Move;
						break;
					case LevelTiles.Ladder:
						this.Debug.Print (this.Tag, "LADDER", 20);
						returnAction.Type = ActionType.Move;
						break;
					default:
					case LevelTiles.Wall:
						this.Debug.Print (this.Tag, "WALL", 20);
						returnAction.Type = ActionType.None;
						break;
					}
				}
			}else {
				switch (aKeyPress){
				case RogueKey.ChangeMode:
					Mode = GameMode.Interact;
					break;
				case RogueKey.Cancel:
					returnAction = null;
					break;
				case RogueKey.Select:
					//temporarily use this to use an item
					//returnAction.Type = ActionType.
					break;
				}

			}
			return returnAction;
		}

		Action InteractMode(RogueKey aKeyPress){
			Action returnAction = new Action (ActionType.None, this.Level.Player.pos.x, this.Level.Player.pos.y);
			if (Input.IsDirectionKey (aKeyPress)) {
				Selection = Input.DirectionKeyToCoordinate (aKeyPress);
			} else {
				switch (aKeyPress){
				case RogueKey.ChangeMode:
					Mode = GameMode.Normal;
					Selection.x = 0;
					Selection.y = 0;
					break;
				case RogueKey.Cancel:
					returnAction = null;
					break;
				case RogueKey.Select:
					if (this.Level.CreatureGrid.GetItem (this.Level.Player.pos + Selection) != null) {
						returnAction.Type = ActionType.Attack;
						returnAction.pos = this.Level.Player.pos + Selection;
					} else if (this.Level.ObjectGrid.GetItem(this.Level.Player.pos + Selection) != null){
						returnAction.Type = ActionType.Interact;
						returnAction.pos = this.Level.Player.pos + Selection;
					} else if (this.Level.ItemGrid.GetItem(this.Level.Player.pos + Selection) != null){
						returnAction.Type = ActionType.PickUp;
						returnAction.pos = this.Level.Player.pos + Selection;
					}
					Mode = GameMode.Normal;
					Selection.x = 0;
					Selection.y = 0;
					break;
				}
			}
			return returnAction;
		}
			

		private bool ProcessAction(Creature aCreature, Action aAction){
			bool validAction = false;
			this.Debug.Print  (this.Tag, "ProcessAction- aCreature.pos.x: " + aCreature.pos.x.ToString() + " aCreature.pos.y: " + aCreature.pos.y.ToString() + " aX: " 
				+ aAction.pos.x.ToString() +" aY: " + aAction.pos.y.ToString() + " Type:" + aAction.Type.ToString(),20);
			switch(aAction.Type){
			case ActionType.Move:
				this.Level.moveCreature (aCreature,aAction.pos.x,aAction.pos.y);
				this.Level.History.AddAction (aAction);
				validAction = true;
				break;
			case ActionType.Interact:
				this.Level.ObjectGrid.GetItem(aAction.pos).Interact (aCreature);
				this.Level.History.AddAction (aAction);
				validAction = true;
				break;
			case ActionType.Attack:
				Creature lTarget = this.Level.getCreature (aAction.pos.x, aAction.pos.y);
				if (lTarget != null) {
					this.Level.History.AddAction (aAction,lTarget);
					if(lTarget.TakeDamage(aCreature.Strength)){
						this.Level.removeCreature (lTarget);
					}
					validAction = true;
				} else {
					validAction = false;
				}
				break;
			case ActionType.PickUp:
				ItemInterface lItem = this.Level.ItemGrid.GetItem (aAction.pos);
				if (lItem != null) {
					aCreature.Inventory.Add (lItem);
					this.Level.History.AddAction (aAction, lItem);
					this.Level.ItemGrid.SetItem (null, aAction.pos);
					validAction = true;
				}
				break;
			default:
			case ActionType.None:
				validAction = false;
				break;
			}
			return validAction;
		}

		private Action AICreatureAction(Creature aCreature){
			Action returnAction = new Action (ActionType.None, aCreature.pos.x, aCreature.pos.y);

			if(this.Level.InLineOfSight(aCreature,this.Level.Player)){
				this.Debug.Print  ( this.Tag, "InLineOfSight Yes",20);
				if (aCreature.pos.x > this.Level.Player.pos.x && aCreature.pos.y < this.Level.Player.pos.y) {
					returnAction.pos = aCreature.pos + Input.DirectionKeyToCoordinate(RogueKey.SouthWest);
				} else if (aCreature.pos.x == this.Level.Player.pos.x && aCreature.pos.y < this.Level.Player.pos.y) {
					returnAction.pos = aCreature.pos + Input.DirectionKeyToCoordinate(RogueKey.South);
				} else if (aCreature.pos.x < this.Level.Player.pos.x && aCreature.pos.y < this.Level.Player.pos.y) {
					returnAction.pos = aCreature.pos + Input.DirectionKeyToCoordinate(RogueKey.SouthEast);
				} else if (aCreature.pos.x > this.Level.Player.pos.x && aCreature.pos.y == this.Level.Player.pos.y) {
					returnAction.pos = aCreature.pos + Input.DirectionKeyToCoordinate(RogueKey.West);
				} else if (aCreature.pos.x < this.Level.Player.pos.x && aCreature.pos.y == this.Level.Player.pos.y) {
					returnAction.pos = aCreature.pos + Input.DirectionKeyToCoordinate(RogueKey.East);
				} else if (aCreature.pos.x > this.Level.Player.pos.x && aCreature.pos.y > this.Level.Player.pos.y) {
					returnAction.pos = aCreature.pos + Input.DirectionKeyToCoordinate(RogueKey.NorthWest);
				} else if (aCreature.pos.x == this.Level.Player.pos.x && aCreature.pos.y > this.Level.Player.pos.y) {
					returnAction.pos = aCreature.pos + Input.DirectionKeyToCoordinate(RogueKey.North);
				} else if (aCreature.pos.x < this.Level.Player.pos.x && aCreature.pos.y > this.Level.Player.pos.y) {
					returnAction.pos = aCreature.pos + Input.DirectionKeyToCoordinate(RogueKey.NorthEast);
				}
			} else {
				this.Debug.Print (this.Tag,"InLineOfSight No",20);
				returnAction.pos = aCreature.pos + Input.DirectionKeyToCoordinate((RogueKey)this.Rand.Next (9));
			}


			if (this.Level.CreatureGrid.GetItem (returnAction.pos) != null) {
				//If there is a creature
				Creature lTarget = this.Level.CreatureGrid.GetItem (returnAction.pos);
				if (lTarget.Group != aCreature.Group) {
					returnAction.Type = ActionType.Attack;
				}
			} else if (this.Level.ObjectGrid.GetItem (returnAction.pos) != null) {
				//if there is an object
				ObjectInterface lObject = this.Level.ObjectGrid.GetItem (returnAction.pos);
				if (lObject.CanWalk ()) {
					returnAction.Type = ActionType.Move;
				} else {
					returnAction.Type = ActionType.Interact;
				}
			} else {
				//else just work it out based on base grid
				switch (this.Level.BaseGrid.GetItem (returnAction.pos)) {
				case LevelTiles.Floor:
					returnAction.Type = ActionType.Move;
					break;
				case LevelTiles.Ladder:
					returnAction.Type = ActionType.Move;
					break;
				default:
				case LevelTiles.Wall:
					returnAction.Type = ActionType.None;
					break;
				}
			}
			return returnAction;
		}

	}
}


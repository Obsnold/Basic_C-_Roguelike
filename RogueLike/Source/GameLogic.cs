using System;
using System.Collections.Generic;

namespace RogueLike
{

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
		}

		public bool GameTick(RogueKey keyPress){
			bool returnValue = true;
			if (keyPress != RogueKey.Cancel) {
				
				Action tempAction = ProcessKeyPress (this.Level.Player, keyPress);
				if (ProcessAction (this.Level.Player, tempAction)) {
					this.Level.History.AddAction (tempAction);
					List<Creature> lCreatureList = this.Level.getCreatureList ();
					lCreatureList.Remove (this.Level.Player);
					if (lCreatureList != null && lCreatureList.Count > 0) {
						foreach (Creature lCreature in lCreatureList) {
							if (!lCreature.IsDead ()) {
								tempAction = AICreatureAction (lCreature);
								ProcessAction (lCreature, tempAction);
							}
							if (this.Level.Player.IsDead()) {
								returnValue = false;
								break;
							}
						}
					}
					this.Level.ComputePlayerFOV ();
					this.Display.printMainScreen (this.Level);
				}
			} else {
				returnValue = false;
			}
			return returnValue;
		}

		Action ProcessKeyPress(Creature aCreature,RogueKey aKeyPress){
			Action returnAction = new Action (ActionType.None, aCreature.pos.x, aCreature.pos.y);

			this.Debug.Print  ( this.Tag, "KEYPRESS:" + aKeyPress.ToString (),20);

			switch(aKeyPress){
			case RogueKey.SouthWest:
				returnAction.pos.x -= 1;
				returnAction.pos.y += 1;
				break;
			case RogueKey.South:
				returnAction.pos.y += 1;
				break;
			case RogueKey.SouthEast:
				returnAction.pos.x += 1;
				returnAction.pos.y += 1;
				break;
			case RogueKey.West:
				returnAction.pos.x -= 1;
				break;
			case RogueKey.East:
				returnAction.pos.x += 1;
				break;
			case RogueKey.NorthWest:
				returnAction.pos.x -= 1;
				returnAction.pos.y -= 1;
				break;
			case RogueKey.North:
				returnAction.pos.y -= 1;
				break;
			case RogueKey.NorthEast:
				returnAction.pos.x += 1;
				returnAction.pos.y -= 1;
				break;
			}

			if(this.Level.CreatureGrid.GetItem(returnAction.pos) != aCreature){
				Creature lTarget = this.Level.CreatureGrid.GetItem (returnAction.pos);
				if (lTarget != null) {
					if (lTarget.Group != aCreature.Group) {
						returnAction.Type = ActionType.Attack;
					}
				} else {
					switch (this.Level.BaseGrid.GetItem (returnAction.pos)) {
						case LevelTiles.Floor:
						ObjectInterface lObject = this.Level.ObjectGrid.GetItem(returnAction.pos);
						if (lObject != null) {
							if (lObject.CanWalk ()) {
								this.Debug.Print (this.Tag, "DOOR_MOVE", 20);
								returnAction.Type = ActionType.Move;
							} else {
								this.Debug.Print (this.Tag, "DOOR_INTERACT", 20);
								returnAction.Type = ActionType.Interact;
							}
						} else {
							this.Debug.Print (this.Tag, "FLOOR_MOVE", 20);
							returnAction.Type = ActionType.Move;
						}
						break;
					case LevelTiles.Ladder:
						this.Debug.Print  ( this.Tag, "LADDER",20);
						returnAction.Type = ActionType.Move;
						break;
					default:
					case LevelTiles.Wall:
						this.Debug.Print  ( this.Tag, "WALL",20);
						returnAction.Type = ActionType.None;
						break;
					}
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
				validAction = true;
				break;
			case ActionType.Interact:
				this.Level.ObjectGrid.GetItem(aAction.pos).Interact (aCreature);
				validAction = true;
				break;
			case ActionType.Attack:
				Creature lTarget = this.Level.getCreature (aAction.pos.x, aAction.pos.y);
				if (lTarget != null) {
					if(lTarget.TakeDamage(aCreature.Strength)){
						this.Level.removeCreature (lTarget);
					}
					validAction = true;
				} else {
					validAction = false;
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
			RogueKey lAIMove = RogueKey.NA;
			if(this.Level.InLineOfSight(aCreature,this.Level.Player)){
				this.Debug.Print  ( this.Tag, "InLineOfSight Yes",20);
				if (aCreature.pos.x > this.Level.Player.pos.x && aCreature.pos.y < this.Level.Player.pos.y) {
					lAIMove = RogueKey.SouthWest;
				} else if (aCreature.pos.x == this.Level.Player.pos.x && aCreature.pos.y < this.Level.Player.pos.y) {
					lAIMove = RogueKey.South;
				} else if (aCreature.pos.x < this.Level.Player.pos.x && aCreature.pos.y < this.Level.Player.pos.y) {
					lAIMove = RogueKey.SouthEast;
				} else if (aCreature.pos.x > this.Level.Player.pos.x && aCreature.pos.y == this.Level.Player.pos.y) {
					lAIMove = RogueKey.West;
				} else if (aCreature.pos.x < this.Level.Player.pos.x && aCreature.pos.y == this.Level.Player.pos.y) {
					lAIMove = RogueKey.East;
				} else if (aCreature.pos.x > this.Level.Player.pos.x && aCreature.pos.y > this.Level.Player.pos.y) {
					lAIMove = RogueKey.NorthWest;
				} else if (aCreature.pos.x == this.Level.Player.pos.x && aCreature.pos.y > this.Level.Player.pos.y) {
					lAIMove = RogueKey.North;
				} else if (aCreature.pos.x < this.Level.Player.pos.x && aCreature.pos.y > this.Level.Player.pos.y) {
					lAIMove = RogueKey.NorthEast;
				}
			} else {
				this.Debug.Print (this.Tag,"InLineOfSight No",20);
				lAIMove = (RogueKey)this.Rand.Next (9);
			}
			return ProcessKeyPress(aCreature, lAIMove);
		}

	}
}


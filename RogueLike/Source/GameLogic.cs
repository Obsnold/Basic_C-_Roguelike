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
			this.Level.moveCreature (this.Level.Player,this.Level.Player.X,this.Level.Player.Y);
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
			Action returnAction = new Action (ActionType.None, aCreature.X, aCreature.Y);

			this.Debug.Print  ( this.Tag, "KEYPRESS:" + aKeyPress.ToString (),20);

			switch(aKeyPress){
			case RogueKey.SouthWest:
				returnAction.X -= 1;
				returnAction.Y += 1;
				break;
			case RogueKey.South:
				returnAction.Y += 1;
				break;
			case RogueKey.SouthEast:
				returnAction.X += 1;
				returnAction.Y += 1;
				break;
			case RogueKey.West:
				returnAction.X -= 1;
				break;
			case RogueKey.East:
				returnAction.X += 1;
				break;
			case RogueKey.NorthWest:
				returnAction.X -= 1;
				returnAction.Y -= 1;
				break;
			case RogueKey.North:
				returnAction.Y -= 1;
				break;
			case RogueKey.NorthEast:
				returnAction.X += 1;
				returnAction.Y -= 1;
				break;
			}

			if(this.Level.CreatureGrid[returnAction.X, returnAction.Y] != aCreature){
				if (this.Level.CreatureGrid[returnAction.X, returnAction.Y] != null) {
					if (this.Level.CreatureGrid [returnAction.X, returnAction.Y].Group != aCreature.Group) {
						returnAction.Type = ActionType.Attack;
					}
				} else {
					switch (this.Level.TileGrid [returnAction.X, returnAction.Y]) {
					case LevelTiles.DoorClosed:
						this.Debug.Print  ( this.Tag, "DOOR_CLOSED",20);
						returnAction.Type = ActionType.Open;
						break;
					case LevelTiles.DoorOpen:
						this.Debug.Print  ( this.Tag, "DOOR_OPEN",20);
						returnAction.Type = ActionType.Move;
						break;
					case LevelTiles.Floor:
						this.Debug.Print  ( this.Tag, "FLOOR",20);
						returnAction.Type = ActionType.Move;
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
			this.Debug.Print  (this.Tag, "ProcessAction- aCreature.X: " + aCreature.X.ToString() + " aCreature.Y: " + aCreature.Y.ToString() + " aX: " 
				+ aAction.X.ToString() +" aY: " + aAction.Y.ToString() + " Type:" + aAction.Type.ToString(),20);
			switch(aAction.Type){
			case ActionType.Move:
				this.Level.moveCreature (aCreature,aAction.X,aAction.Y);
				validAction = true;
				break;
			case ActionType.Open:
				this.Level.TileGrid [aAction.X, aAction.Y] = LevelTiles.DoorOpen;
				validAction = true;
				break;
			case ActionType.Attack:
				Creature lTarget = this.Level.getCreature (aAction.X, aAction.Y);
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
				if (aCreature.X > this.Level.Player.X && aCreature.Y < this.Level.Player.Y) {
					lAIMove = RogueKey.SouthWest;
				} else if (aCreature.X == this.Level.Player.X && aCreature.Y < this.Level.Player.Y) {
					lAIMove = RogueKey.South;
				} else if (aCreature.X < this.Level.Player.X && aCreature.Y < this.Level.Player.Y) {
					lAIMove = RogueKey.SouthEast;
				} else if (aCreature.X > this.Level.Player.X && aCreature.Y == this.Level.Player.Y) {
					lAIMove = RogueKey.West;
				} else if (aCreature.X < this.Level.Player.X && aCreature.Y == this.Level.Player.Y) {
					lAIMove = RogueKey.East;
				} else if (aCreature.X > this.Level.Player.X && aCreature.Y > this.Level.Player.Y) {
					lAIMove = RogueKey.NorthWest;
				} else if (aCreature.X == this.Level.Player.X && aCreature.Y > this.Level.Player.Y) {
					lAIMove = RogueKey.North;
				} else if (aCreature.X < this.Level.Player.X && aCreature.Y > this.Level.Player.Y) {
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


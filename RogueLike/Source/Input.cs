using System;

namespace RogueLike
{
	public enum RogueKey{
		North,
		South,
		East,
		West,
		NorthEast,
		NorthWest,
		SouthEast,
		SouthWest,
		Select,
		Cancel,
		ChangeMode,
		NA
	}

	public static class Input
	{
		public static RogueKey getKey(ConsoleKey aKeyPress)
		{
			RogueKey returnKey;

			switch (aKeyPress) {
			case ConsoleKey.NumPad1:
			case ConsoleKey.Z:
				returnKey = RogueKey.SouthWest;
				break;
			case ConsoleKey.NumPad2:
			case ConsoleKey.DownArrow:
			case ConsoleKey.X:
				returnKey = RogueKey.South;
				break;
			case ConsoleKey.NumPad3:
			case ConsoleKey.C:
				returnKey = RogueKey.SouthEast;
				break;
			case ConsoleKey.NumPad4:
			case ConsoleKey.LeftArrow:
			case ConsoleKey.A:
				returnKey = RogueKey.West;
				break;
			case ConsoleKey.NumPad5:
			case ConsoleKey.S:
				returnKey = RogueKey.NA;
				break;
			case ConsoleKey.NumPad6:
			case ConsoleKey.RightArrow:
			case ConsoleKey.D:
				returnKey = RogueKey.East;
				break;
			case ConsoleKey.NumPad7:
			case ConsoleKey.Q:
				returnKey = RogueKey.NorthWest;
				break;
			case ConsoleKey.NumPad8:
			case ConsoleKey.UpArrow:
			case ConsoleKey.W:
				returnKey = RogueKey.North;
				break;
			case ConsoleKey.NumPad9:
			case ConsoleKey.E:
				returnKey = RogueKey.NorthEast;
				break;
			case ConsoleKey.Enter:
				returnKey = RogueKey.Select;
				break;
			case ConsoleKey.Backspace:
			case ConsoleKey.Escape:
				returnKey = RogueKey.Cancel;
				break;
			case ConsoleKey.Spacebar:
				returnKey = RogueKey.ChangeMode;
				break;
			default:
				returnKey = RogueKey.NA;
				break;
			}
			return returnKey;
		}


		public static Coordinate DirectionKeyToCoordinate(RogueKey aKeyPress){
			Coordinate lReturn;
			lReturn.x = 0;
			lReturn.y = 0;
			switch(aKeyPress){
			case RogueKey.SouthWest:
				lReturn.x = -1;
				lReturn.y = 1;
				break;
			case RogueKey.South:
				lReturn.y = 1;
				break;
			case RogueKey.SouthEast:
				lReturn.x = 1;
				lReturn.y = 1;
				break;
			case RogueKey.West:
				lReturn.x = -1;
				break;
			case RogueKey.East:
				lReturn.x = 1;
				break;
			case RogueKey.NorthWest:
				lReturn.x = -1;
				lReturn.y = -1;
				break;
			case RogueKey.North:
				lReturn.y = -1;
				break;
			case RogueKey.NorthEast:
				lReturn.x = 1;
				lReturn.y = -1;
				break;
			}
			return lReturn;
		}

		public static bool IsDirectionKey(RogueKey aKeyPress){
			bool lReturn = false;
			if ((aKeyPress == RogueKey.SouthWest) ||
			   (aKeyPress == RogueKey.South) ||
			   (aKeyPress == RogueKey.SouthEast) ||
			   (aKeyPress == RogueKey.West) ||
			   (aKeyPress == RogueKey.East) ||
			   (aKeyPress == RogueKey.NorthWest) ||
			   (aKeyPress == RogueKey.North) ||
			   (aKeyPress == RogueKey.NorthEast)) {
				lReturn = true;
			}
			return lReturn;
		}
	}
}


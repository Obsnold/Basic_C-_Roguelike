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
		NA
	}

	public class Input
	{
		public RogueKey getKey(ConsoleKey aKeyPress)
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
			default:
				returnKey = RogueKey.NA;
				break;
			}
			return returnKey;
		}
	}
}


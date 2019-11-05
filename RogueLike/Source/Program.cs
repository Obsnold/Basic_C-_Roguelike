using System;

namespace RogueLike
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			RogueKey keyPressed = new RogueKey();
			Display mDisplay = new Display ();
			GameLogic gLogic = new GameLogic (mDisplay);

			do {
				keyPressed = Input.getKey(Console.ReadKey(true).Key);

			} while(gLogic.GameTick(keyPressed));
		}
	}
}

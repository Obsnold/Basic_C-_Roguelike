using System;

namespace RogueLike
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			RogueKey keyPressed = new RogueKey();
			Display mDisplay = new Display ();
			Input mInput = new Input ();
			GameLogic gLogic = new GameLogic (mDisplay);

			do {
				keyPressed = mInput.getKey(Console.ReadKey(true).Key);

			} while(gLogic.GameTick(keyPressed));
		}
	}
}

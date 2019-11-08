using System;

namespace RogueLike
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Display mDisplay = new Display ();
			GameLogic gLogic = new GameLogic (mDisplay);

			do {
			} while(gLogic.GameTick());
		}
	}
}

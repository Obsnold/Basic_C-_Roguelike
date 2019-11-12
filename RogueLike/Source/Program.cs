using System;

namespace RogueLike
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			GameLogic gLogic = new GameLogic ();

			do {
			} while(gLogic.GameTick());
		}
	}
}

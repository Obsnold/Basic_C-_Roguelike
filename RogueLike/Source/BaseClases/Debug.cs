using System;

namespace RogueLike
{
	public class Debug
	{
		private static Debug mDebug;
		int counter = 0;
		int total = 0;
		bool debug = true;
		int debugLevel = 10;

		private Debug(){
		}

		public static Debug Instance
		{
			get{
				if (mDebug == null) {
					mDebug = new Debug ();
				} 
				return mDebug;
			}
		}

		public void Print(String aTag, String aText,int aLevel){
			if (debug && (aLevel < debugLevel)) {
				counter++;
				total++;
				if (counter > 10) {
					counter = 0;
				}
				Console.SetCursorPosition (0, 25+ counter);
				Console.WriteLine (total.ToString() + " " + aTag + ": " + aText);
			}
		}
	}
}


using System;
using System.Collections.Generic;

namespace RogueLike
{
	public class History
	{
		public List<String> HistoryList;
		public static int HistoryLength = 100;

		public History ()
		{
			this.HistoryList = new List<String> (HistoryLength);
		}

		public void AddString(String aString){
			if (this.HistoryList.Count > HistoryLength) {
				this.HistoryList.RemoveAt (0);
			}
			this.HistoryList.Add (aString);
		}
	}
}


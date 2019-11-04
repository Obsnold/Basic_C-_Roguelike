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

		public void AddAction(Action aAction){
			if (this.HistoryList.Count > HistoryLength) {
				this.HistoryList.RemoveAt (0);
			}

			switch (aAction.Type) {
			case ActionType.Attack:
				this.HistoryList.Add ("Attack!");
				break;
			case ActionType.Move:
				this.HistoryList.Add ("Move!");
				break;
			case ActionType.Interact:
				this.HistoryList.Add ("Interact!");
				break;
			case ActionType.None:
				this.HistoryList.Add ("Do Nothing.");
				break;
			}
		}
	}
}


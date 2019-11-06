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

		public void AddAction(Action aAction, Creature aTarget){
			if (this.HistoryList.Count > HistoryLength) {
				this.HistoryList.RemoveAt (0);
			}

			switch (aAction.Type) {
			case ActionType.Attack:
				this.HistoryList.Add ("Attack " + aTarget.Name);
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

		public void AddAction(Action aAction, ItemInterface aItem){
			if (this.HistoryList.Count > HistoryLength) {
				this.HistoryList.RemoveAt (0);
			}

			switch (aAction.Type) {
			case ActionType.Attack:
				this.HistoryList.Add ("Attack " + aItem.GetDescription());
				break;
			case ActionType.Move:
				this.HistoryList.Add ("Move!");
				break;
			case ActionType.Interact:
				this.HistoryList.Add ("Interact "  + aItem.GetDescription());
				break;
			case ActionType.PickUp:
				this.HistoryList.Add ("PickUp "  + aItem.GetDescription());
				break;
			case ActionType.Drop:
				this.HistoryList.Add ("Drop "  + aItem.GetDescription());
				break;
			case ActionType.None:
				this.HistoryList.Add ("Do Nothing.");
				break;
			}
		}


		public void AddString(String aString){
			if (this.HistoryList.Count > HistoryLength) {
				this.HistoryList.RemoveAt (0);
			}
			this.HistoryList.Add (aString);
		}
	}
}


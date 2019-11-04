using System;

namespace RogueLike
{
	public enum ActionType{
		Move,
		Attack,
		Interact,
		None
	}

	public class Action{
		public ActionType Type;
		public int X;
		public int Y;

		public Action(ActionType aType, int aX, int aY){
			this.Type = aType;
			this.X = aX;
			this.Y = aY;
		}
	}
}


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
		public Coordinate pos;

		public Action(ActionType aType, int aX, int aY){
			this.Type = aType;
			this.pos.x = aX;
			this.pos.y = aY;
		}
	}
}


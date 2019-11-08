using System;

namespace RogueLike
{
	public class Enemy : Actor
	{
		
		public Enemy (String aName, int aX, int aY, int aHealth ,int aStrength, int aGroup, Level aLevel):
		base (aName, aX, aY, aHealth , aStrength, aGroup, aLevel)
		{
		}

		public override Action TakeTurn (){
			Direction lMove = Direction.NA;

			if(this.Level.InLineOfSight(this,this.Level.Player)){
				if (this.pos.x > this.Level.Player.pos.x && this.pos.y < this.Level.Player.pos.y) {
					lMove = Direction.SouthWest;
				} else if (this.pos.x == this.Level.Player.pos.x && this.pos.y < this.Level.Player.pos.y) {
					lMove = Direction.South;
				} else if (this.pos.x < this.Level.Player.pos.x && this.pos.y < this.Level.Player.pos.y) {
					lMove = Direction.SouthEast;
				} else if (this.pos.x > this.Level.Player.pos.x && this.pos.y == this.Level.Player.pos.y) {
					lMove = Direction.West;
				} else if (this.pos.x < this.Level.Player.pos.x && this.pos.y == this.Level.Player.pos.y) {
					lMove = Direction.East;
				} else if (this.pos.x > this.Level.Player.pos.x && this.pos.y > this.Level.Player.pos.y) {
					lMove = Direction.NorthWest;
				} else if (this.pos.x == this.Level.Player.pos.x && this.pos.y > this.Level.Player.pos.y) {
					lMove = Direction.North;
				} else if (this.pos.x < this.Level.Player.pos.x && this.pos.y > this.Level.Player.pos.y) {
					lMove = Direction.NorthEast;
				}
			} else {
				lMove = (Direction)StaticRandom.Instance.Next (9);
			}
				
			return new MoveAction(this.Level,this,lMove);
		}
	}
}


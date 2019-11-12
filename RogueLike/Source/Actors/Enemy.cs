using System;

namespace RogueLike
{
	public class Enemy : Actor
	{
		
		public Enemy (String aName, int aX, int aY, int aHealth ,int aStrength, int aGroup):
		base (aName, aX, aY, aHealth , aStrength, aGroup)
		{
		}

		public override Action TakeTurn (){
			Direction lMove = Direction.NA;
			Coordinate lPlayerPos = level.Player.GetPos ();
			if(this.level.InLineOfSight(this,lPlayerPos)){
				if (this.pos.x > lPlayerPos.x && this.pos.y < lPlayerPos.y) {
					lMove = Direction.SouthWest;
				} else if (this.pos.x == lPlayerPos.x && this.pos.y < lPlayerPos.y) {
					lMove = Direction.South;
				} else if (this.pos.x < lPlayerPos.x && this.pos.y < lPlayerPos.y) {
					lMove = Direction.SouthEast;
				} else if (this.pos.x > lPlayerPos.x && this.pos.y == lPlayerPos.y) {
					lMove = Direction.West;
				} else if (this.pos.x < lPlayerPos.x && this.pos.y == lPlayerPos.y) {
					lMove = Direction.East;
				} else if (this.pos.x > lPlayerPos.x && this.pos.y > lPlayerPos.y) {
					lMove = Direction.NorthWest;
				} else if (this.pos.x == lPlayerPos.x && this.pos.y > lPlayerPos.y) {
					lMove = Direction.North;
				} else if (this.pos.x < lPlayerPos.x && this.pos.y > lPlayerPos.y) {
					lMove = Direction.NorthEast;
				}
			} else {
				lMove = (Direction)StaticRandom.Instance.Next (9);
			}
				
			return new MoveAction(this,lMove);
		}
	}
}


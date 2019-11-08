using System;

namespace RogueLike
{
	public struct Coordinate
	{
		public int x;
		public int y;

		public Coordinate (int aX, int aY)
		{
			x = aX;
			y = aY;
		}

		public static Coordinate operator+ (Coordinate aA, Coordinate aB){
			Coordinate lResult;
			lResult.x = aA.x + aB.x;
			lResult.y = aA.y + aB.y;
			return lResult;
		}

		public static Coordinate operator+ (Coordinate aA, Direction aB){
			Coordinate lResult;

			lResult.x = aA.x;
			lResult.y = aA.y;

			switch(aB){
			case Direction.SouthWest:
				lResult.x -= 1;
				lResult.y += 1;
				break;
			case Direction.South:
				lResult.y += 1;
				break;
			case Direction.SouthEast:
				lResult.x += 1;
				lResult.y += 1;
				break;
			case Direction.West:
				lResult.x -= 1;
				break;
			case Direction.East:
				lResult.x += 1;
				break;
			case Direction.NorthWest:
				lResult.x -= 1;
				lResult.y -= 1;
				break;
			case Direction.North:
				lResult.y -= 1;
				break;
			case Direction.NorthEast:
				lResult.x += 1;
				lResult.y -= 1;
				break;
			}
				
			return lResult;
		}

		public static Coordinate operator- (Coordinate aA, Coordinate aB){
			Coordinate lResult;
			lResult.x = aA.x - aB.x;
			lResult.y = aA.y - aB.y;
			return lResult;
		}

		public static bool operator== (Coordinate aA, Coordinate aB){
			bool lResult = false;
			if ((aA.x == aB.x) && (aA.y == aB.y)) {
				lResult = true;
			}
			return lResult;
		}

		public static bool operator!= (Coordinate aA, Coordinate aB){
			bool lResult = false;
			if ((aA.x != aB.x) || (aA.y != aB.y)) {
				lResult = true;
			}
			return lResult;
		}

		public static bool operator< (Coordinate aA, Coordinate aB){
			bool lResult = false;
			if ((aA.x < aB.x) && (aA.y < aB.y)) {
				lResult = true;
			}
			return lResult;
		}

		public static bool operator> (Coordinate aA, Coordinate aB){
			bool lResult = false;
			if ((aA.x > aB.x) && (aA.y > aB.y)) {
				lResult = true;
			}
			return lResult;
		}

		public static bool operator<= (Coordinate aA, Coordinate aB){
			bool lResult = false;
			if ((aA.x <= aB.x) && (aA.y <= aB.y)) {
				lResult = true;
			}
			return lResult;
		}

		public static bool operator>= (Coordinate aA, Coordinate aB){
			bool lResult = false;
			if ((aA.x >= aB.x) && (aA.y >= aB.y)) {
				lResult = true;
			}
			return lResult;
		}

		public override bool Equals (object obj)
		{
			if (!(obj is Coordinate))
				return false;

			Coordinate mys = (Coordinate) obj;

			if (this.x == mys.x &&
			    this.y == mys.y) {
				return true;
			} else {
				return false;
			}
		}

		public override int GetHashCode ()
		{
			return new Tuple<int,int>(this.x, this.y).GetHashCode();
		}
	}
}


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
			throw new System.NotImplementedException ();
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
	}
}


using System;

namespace RogueLike
{
	public struct Coodinate
	{
		public int x;
		public int y;

		public Coodinate (int aX, int aY)
		{
			x = aX;
			y = aY;
		}

		public static Coodinate operator+ (Coodinate aA, Coodinate aB){
			Coodinate lResult;
			lResult.x = aA.x + aB.x;
			lResult.y = aA.y + aB.y;
			return lResult;
		}

		public static Coodinate operator- (Coodinate aA, Coodinate aB){
			Coodinate lResult;
			lResult.x = aA.x - aB.x;
			lResult.y = aA.y - aB.y;
			return lResult;
		}

		public static bool operator== (Coodinate aA, Coodinate aB){
			bool lResult = false;
			if ((aA.x == aB.x) && (aA.y == aB.y)) {
				lResult = true;
			}
			return lResult;
		}

		public static bool operator!= (Coodinate aA, Coodinate aB){
			bool lResult = false;
			if ((aA.x != aB.x) || (aA.y != aB.y)) {
				lResult = true;
			}
			return lResult;
		}

		public static bool operator< (Coodinate aA, Coodinate aB){
			bool lResult = false;
			if ((aA.x < aB.x) && (aA.y < aB.y)) {
				lResult = true;
			}
			return lResult;
		}

		public static bool operator> (Coodinate aA, Coodinate aB){
			bool lResult = false;
			if ((aA.x > aB.x) && (aA.y > aB.y)) {
				lResult = true;
			}
			return lResult;
		}

		public static bool operator<= (Coodinate aA, Coodinate aB){
			bool lResult = false;
			if ((aA.x <= aB.x) && (aA.y <= aB.y)) {
				lResult = true;
			}
			return lResult;
		}

		public static bool operator>= (Coodinate aA, Coodinate aB){
			bool lResult = false;
			if ((aA.x >= aB.x) && (aA.y >= aB.y)) {
				lResult = true;
			}
			return lResult;
		}
	}
}


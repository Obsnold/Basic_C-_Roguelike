using System;

namespace RogueLike
{
	public class Grid<T>
	{
		private T[,] Array;
		private int Width;
		private int Height;

		public Grid (int aWidth, int aHeight)
		{
			this.Array = new T[aWidth,aHeight];
			this.Width = aWidth;
			this.Height = aHeight;
		}

		public T GetItem(int aX, int aY){
			if (InBounds(aX,aY)) {
				return Array [aX, aY];
			} else {
				return default(T);
			}
		}

		public T GetItem(Coodinate aCoord){
			return GetItem(aCoord.x,aCoord.y);
		}

		public bool SetItem(T aItem, int aX, int aY){
			bool lResult = false;
			if(InBounds(aX, aY)){
				Array [aX, aY] = aItem;
				lResult = true;
			} 
			return lResult;
		}

		public bool SetItem(T aItem, Coodinate aCoord){
			return SetItem(aItem,aCoord.x,aCoord.y);
		}

		public bool InBounds(int aX, int aY){
			bool lInBounds = true;
			if((aX < 0) || (aX >= this.Width) ||
				(aY < 0) || (aY >= this.Height)){
				lInBounds = false;
			}
			return lInBounds;
		}

		public bool InBounds(Coodinate aCoord){
			return InBounds (aCoord);
		}
	}
}


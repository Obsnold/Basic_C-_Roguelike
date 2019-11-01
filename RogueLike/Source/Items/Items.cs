using System;

namespace RogueLike
{
	public enum ItemType{
		Food,
		Weapon,
		Trap,
		None
	}

	public class Item
	{
		public ItemType mType;
		public int mX;
		public int mY;

		public Item ()
		{

		}
	}
}


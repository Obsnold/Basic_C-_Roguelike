using System;

namespace RogueLike
{
	public interface ItemInterface
	{
		bool Interact(Creature aCreature);
		String GetDescription();
	}
}


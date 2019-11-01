using System;

namespace RogueLike
{
	public interface ObjectInterface
	{
		bool Interact(Creature aCreature);
		bool TakeDamage(int aDamage);
		bool CanWalk();
		bool CanSeePast();
	}
}


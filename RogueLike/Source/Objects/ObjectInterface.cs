using System;

namespace RogueLike
{
	public interface ObjectInterface
	{
		bool Interact(Actor aActor);
		bool TakeDamage(int aDamage);
		bool CanWalk();
		bool CanSeePast();
		DisplayTile GetDisplayTile();
		Action DefaultAction(Level aLevel, Actor aActor);
	}
}


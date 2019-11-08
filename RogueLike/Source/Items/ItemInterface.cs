using System;

namespace RogueLike
{
	public interface ItemInterface
	{
		bool Interact(Actor aActor);
		String GetDescription();
	}
}


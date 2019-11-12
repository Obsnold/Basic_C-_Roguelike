using System;

namespace RogueLike
{
	public interface ItemInterface
	{
		bool Interact(Actor aActor);
		String GetDescription();
		Coordinate GetPosition();
		void SetPosition(Coordinate aCoordinate);
	}
}


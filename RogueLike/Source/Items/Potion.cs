using System;
using System.Collections.Generic;

namespace RogueLike
{
	public class Potion : ItemInterface
	{
		Coordinate pos;
		int Power = 2;
		String Name;

		public Potion ()
		{
			this.Name = "Health Potion";
		}

		public bool Interact(Actor aActor){
			aActor.Health += this.Power;

			return true;
		}

		public String GetDescription(){
			return this.Name;
		}

		public Coordinate GetPosition(){
			return this.pos;
		}

		public void SetPosition(Coordinate aCoord){
			this.pos = aCoord;
		}
	}
}


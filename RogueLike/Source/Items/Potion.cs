using System;
using System.Collections.Generic;

namespace RogueLike
{
	public class Potion : ItemInterface
	{
		int Power = 2;
		int Weight = 1;
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
	}
}


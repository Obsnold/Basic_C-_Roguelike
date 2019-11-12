using System;
using System.Collections.Generic;

namespace RogueLike
{
	public class Potion : ItemInterface
	{
		Use Use;
		String Name;

		public Potion ()
		{
			this.Use = 
			this.Name = "Potion";
		}

		public bool Interact(Actor aActor){
			aActor.Health += this.Power;

			return true;
		}
			
	}
}


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RogueLike
{
	public class ActorTemplate
	{
		public String Name;
		public int MaxHealth;
		public int Str;
		public int Con;
		public int Dex;
		public int Ref;
		public int Int;
		public int Wis;

		public List<Use> Abilities;
		public Tags Tags;

		public int Level;

		public ActorTemplate (){
			this.Abilities = new List<Use> ();
			this.Tags = new Tags ();
		}
	}
}


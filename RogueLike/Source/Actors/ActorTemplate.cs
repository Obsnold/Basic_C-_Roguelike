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
		public int Dex;
		public int Int;

		public List<Use> Abilities;
		public Tags Tags;

		public int Level;

		public ActorTemplate (){
			this.Abilities = new List<Use> ();
			this.Tags = new Tags ();
		}


		public bool AttributeCheck(int aBaseStat, int aCheckValue, int aModifier){
			//times max by 100 to get bigger rand range
			int lResult = StaticRandom.Instance.Next (0,100);
			int lCheck = aCheckValue - (aBaseStat + aModifier);
			if (lResult >= lCheck) {
				return true;
			} else {
				return false;
			}
		}
	}
}


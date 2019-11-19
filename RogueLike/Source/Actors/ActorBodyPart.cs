using System;

namespace RogueLike
{
	public class ActorBodyPart
	{
		public Item Equiped = null;
		String BodyPart;

		public ActorBodyPart (String aBodyPart)
		{
			BodyPart = aBodyPart;
		}

		public bool Equip(Item aItem){
			bool lResult = false;
			if(aItem.EquipTo.Equals(BodyPart)){
				this.Equiped = aItem;
				lResult = true;
			}
			return lResult;
		}
	}
}


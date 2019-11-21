using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace RogueLike
{
	public class ActorGenerator
	{
		ConfigData Config = ConfigData.Instance;

		public bool PopulateLevel(Level aLevel){
			for (int i = 0; i < 5; i++) {
				room lTempRoom = aLevel.RoomList[StaticRandom.Instance.Next(1,aLevel.RoomList.Count)];
				Actor lActor = new Enemy (Config.ActorNames[StaticRandom.Instance.Next(0,this.Config.ActorNames.Count)],
					GenActorTemplate(0),
					StaticRandom.Instance.Next (lTempRoom.x, lTempRoom.x + lTempRoom.w),
					StaticRandom.Instance.Next (lTempRoom.y, lTempRoom.y + lTempRoom.h), 1);
				if (!lActor.SetPos(lActor.GetPos())) {
					//debugPrint ("Cannot place Actor");
				}
			}

			//place player
			aLevel.Player = new Player("Player", this.Config.PlayerTemplate, aLevel.Width / 2, aLevel.Height/2, 0);
			aLevel.Player.SetPos (aLevel.Player.GetPos());
			return true;
		}

		public ActorTemplate GenActorTemplate(int aLevel){
			List<string> keyList = new List<string>(this.Config.ActorDic.Keys);
			return this.Config.ActorDic[keyList[StaticRandom.Instance.Next(0, keyList.Count)]];
				//[StaticRandom.Instance.Next(0,this.Config.ActorLevelList.Count)];
		}
	}
}


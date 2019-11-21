using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace RogueLike
{
	public class ActorGenerator
	{
		List<ActorTemplate> ActorLevelList;
		List<String> ActorNames;
		ActorTemplate PlayerTemplate;
		Debug debug = Debug.Instance;

		public ActorGenerator ()
		{
			ActorLevelList = new List<ActorTemplate>();
			ActorNames = new List<string>();
			PlayerTemplate = new ActorTemplate ();

			foreach (string lFile in Directory.EnumerateFiles("/home/terry/Documents/Personal/roguelike/RogueLike/RogueLike/Data/Enemies/Template", "*.xml"))
			{
				ActorTemplate lActorTemplate = new ActorTemplate ();
				if (ParseXMLTemplateFile (lFile, lActorTemplate)) {
					ActorLevelList.Add (lActorTemplate);
				}
			}

			foreach (string lFile in Directory.EnumerateFiles("/home/terry/Documents/Personal/roguelike/RogueLike/RogueLike/Data/Enemies/Names", "*.xml"))
			{
				ParseXMLNameFile (lFile);
			}

			ParseXMLTemplateFile ("/home/terry/Documents/Personal/roguelike/RogueLike/RogueLike/Data/Player/Player.xml", this.PlayerTemplate);
		}

		public bool PopulateLevel(Level aLevel){
			for (int i = 0; i < 5; i++) {
				room lTempRoom = aLevel.RoomList[StaticRandom.Instance.Next(1,aLevel.RoomList.Count)];
				Actor lActor = new Enemy (this.ActorNames[StaticRandom.Instance.Next(0,this.ActorNames.Count)],
					GenActorTemplate(0),
					StaticRandom.Instance.Next (lTempRoom.x, lTempRoom.x + lTempRoom.w),
					StaticRandom.Instance.Next (lTempRoom.y, lTempRoom.y + lTempRoom.h), 1);
				if (!lActor.SetPos(lActor.GetPos())) {
					//debugPrint ("Cannot place Actor");
				}
			}

			//place player
			aLevel.Player = new Player("Player", this.PlayerTemplate, aLevel.Width / 2, aLevel.Height/2, 0);
			aLevel.Player.SetPos (aLevel.Player.GetPos());
			return true;
		}

		public ActorTemplate GenActorTemplate(int aLevel){
			return ActorLevelList[StaticRandom.Instance.Next(0,ActorLevelList.Count)];
		}

		bool ParseXMLNameFile(String aFile){
			bool lResult = false;
			XmlDocument doc = new XmlDocument();
			doc.Load(aFile);
			foreach (XmlNode lNode in doc.DocumentElement.ChildNodes) {
				if(lNode.Name == "Name"){
					this.ActorNames.Add (lNode.InnerText);
				}
			}
			return lResult;
		}

		bool ParseXMLTemplateFile(String aFile, ActorTemplate aTemplate){
			bool lResult = false;
			XmlDocument doc = new XmlDocument();
			doc.Load(aFile);
			foreach (XmlNode lNode in doc.DocumentElement.ChildNodes) {
				lResult = ParseXMLTemplateNode (lNode, aTemplate);
			}
			return lResult;
		}

		bool ParseXMLTemplateNode(XmlNode aNode, ActorTemplate aTemplate){
			bool lResult = true;
			switch (aNode.Name) {
			case "Name":
				aTemplate.Name = aNode.InnerText;
				break;
			case "Level":
				aTemplate.Level = Convert.ToInt32(aNode.InnerText);
				break;
			case "MaxHealth":
				aTemplate.MaxHealth = Convert.ToInt32(aNode.InnerText);
				break;
			case "Str":
				aTemplate.Str = Convert.ToInt32(aNode.InnerText);
				break;
			case "Con":
				aTemplate.Con = Convert.ToInt32(aNode.InnerText);
				break;
			case "Dex":
				aTemplate.Dex = Convert.ToInt32(aNode.InnerText);
				break;
			case "Ref":
				aTemplate.Ref = Convert.ToInt32(aNode.InnerText);
				break;
			case "Int":
				aTemplate.Int = Convert.ToInt32(aNode.InnerText);
				break;
			case "Wis":
				aTemplate.Wis = Convert.ToInt32(aNode.InnerText);
				break;
			case "Item":
				//aTemplate.Name = aNode.InnerText;
				break;
			case "Tag":
				if (aNode.Value != null) {
					aTemplate.Tags.AddTag(aNode.InnerText);
				}
				break;
			default:
				lResult = false;
				break;
			}
			return lResult;
		}
	}
}


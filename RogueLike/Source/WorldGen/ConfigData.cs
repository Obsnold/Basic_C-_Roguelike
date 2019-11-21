using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace RogueLike
{
	public class ConfigData
	{
		static ConfigData mConfigData;

		public Dictionary<String,ActorTemplate> ActorDic;
		public Dictionary<String,Item> ItemDic;
		public List<String> ActorNames;
		public ActorTemplate PlayerTemplate;
		static String DataLocation = "/home/terry/Documents/Personal/roguelike/RogueLike/RogueLike/Data/";

		public static ConfigData Instance
		{
			get{
				if (mConfigData == null) {
					mConfigData = new ConfigData ();
				} 
				return mConfigData;
			}
		}


		public ConfigData ()
		{
			ActorDic = new Dictionary<string, ActorTemplate> ();
			ItemDic = new Dictionary<string, Item> ();
			ActorNames = new List<string>();
			PlayerTemplate = new ActorTemplate ();

			//Get Actor Names
			foreach (string lFile in Directory.EnumerateFiles(DataLocation + "Enemies/Names", "*.xml"))
			{
				ParseXMLNameFile (lFile);
			}

			//Get Enemy Templates
			foreach (string lFile in Directory.EnumerateFiles(DataLocation + "Enemies/Template", "*.xml"))
			{
				ActorTemplate lActorTemplate = new ActorTemplate ();
				if (ParseXMLActorTemplateFile (lFile, lActorTemplate)) {
					ActorDic.Add (lActorTemplate.Name,lActorTemplate);
				}
			}

			//Get Player Templates
			ParseXMLActorTemplateFile (DataLocation + "Player/Player.xml", this.PlayerTemplate);

			//Get Templates Item
			foreach (string lFile in Directory.EnumerateFiles(DataLocation + "Items", "*.xml"))
			{
				Item lItem = null;
				if (ParseXMLItemFile (lFile, out lItem)) {
					ItemDic.Add (lItem.GetName(),lItem);
				}
			}
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

		bool ParseXMLItemFile(String aFile, out Item aItem){
			bool lResult = true;
			XmlDocument doc = new XmlDocument();
			doc.Load(aFile);
			String lName = null;
			String lEquipTo= null;
			Attack lAttack = null;
			Defend lDefend = null;
			foreach (XmlNode lNode in doc.DocumentElement.ChildNodes) {
				switch (lNode.Name) {
				case "Name":
					lName = lNode.InnerText;
					break;
				case "EquipTo":
					lEquipTo = lNode.InnerText;
					break;
				case "Attack":
					int lMax = 0;
					int lMin = 0;
					foreach (XmlNode l1Node in lNode.ChildNodes) {
						switch (l1Node.Name) {
						case "minDamage":
							lMin = Convert.ToInt32(l1Node.InnerText);
							break;
						case "maxDamage":
							lMax = Convert.ToInt32(l1Node.InnerText);
							break;
						}
					}
					lAttack = new Attack (lMin, lMax);
					break;
				case "Defend":
					int lCon = 0;
					int lRef = 0;
					int lWis = 0;
					foreach (XmlNode l1Node in lNode.ChildNodes) {
						switch (l1Node.Name) {
						case "Con":
							lCon = Convert.ToInt32(l1Node.InnerText);
							break;
						case "Ref":
							lRef = Convert.ToInt32(l1Node.InnerText);
							break;
						case "Wis":
							lWis = Convert.ToInt32(l1Node.InnerText);
							break;
						}
					}
					lDefend = new Defend (lCon, lRef, lWis);
					break;
				case "Use":
					
					break;
				default:
					lResult = false;
					break;
				}
			}
			if (lName != null && lEquipTo != null) {
				aItem = new Item (lName, lAttack, lDefend, aEquipTo: lEquipTo);
			} else {
				aItem = null;
			}
			return lResult;
		}

		bool ParseXMLActorTemplateFile(String aFile, ActorTemplate aTemplate){
			bool lResult = true;
			XmlDocument doc = new XmlDocument();
			doc.Load(aFile);
			foreach (XmlNode lNode in doc.DocumentElement.ChildNodes) {
				switch (lNode.Name) {
				case "Name":
					aTemplate.Name = lNode.InnerText;
					break;
				case "Level":
					aTemplate.Level = Convert.ToInt32(lNode.InnerText);
					break;
				case "MaxHealth":
					aTemplate.MaxHealth = Convert.ToInt32(lNode.InnerText);
					break;
				case "Str":
					aTemplate.Str = Convert.ToInt32(lNode.InnerText);
					break;
				case "Con":
					aTemplate.Con = Convert.ToInt32(lNode.InnerText);
					break;
				case "Dex":
					aTemplate.Dex = Convert.ToInt32(lNode.InnerText);
					break;
				case "Ref":
					aTemplate.Ref = Convert.ToInt32(lNode.InnerText);
					break;
				case "Int":
					aTemplate.Int = Convert.ToInt32(lNode.InnerText);
					break;
				case "Wis":
					aTemplate.Wis = Convert.ToInt32(lNode.InnerText);
					break;
				case "Item":
					//aTemplate.Name = aNode.InnerText;
					break;
				case "Tag":
					if (lNode.Value != null) {
						aTemplate.Tags.AddTag(lNode.InnerText);
					}
					break;
				default:
					lResult = false;
					break;
				}
			}
			return lResult;
		}
	}
}


using System;
using System.Collections.Generic;

namespace RogueLike
{
	public class Tags
	{
		public Dictionary<String,String> TagTable;

		public Tags (){
			this.TagTable = new Dictionary<string, string> ();
		}

		public bool HasTag(String aTag){
			String lValue;
			return this.TagTable.TryGetValue(aTag,out lValue);
		}

		public bool AddTag(String aTag){
			bool lResult = false;
			if (!HasTag(aTag)) {
				this.TagTable.Add (aTag, aTag);
				lResult = true;
			}
			return lResult;
		}

	}
}


using System;

namespace RogueLike
{
	public class ActionResult
	{
		public bool Result;
		public String ActionText;
		public Action AlternateAction;

		public ActionResult (bool aResult)
		{
			this.Result = aResult;
		}

		public void Alternate(Action aAction){
			AlternateAction = aAction;
			this.Result = false;
			return;
		}

		public bool Success(String aText = null){
			this.Result = true;
			this.ActionText = aText;
			return this.Result;
		}

		public bool Failure(String aText = null){
			this.Result = false;
			this.ActionText = aText;
			return this.Result;
		}
	}
}


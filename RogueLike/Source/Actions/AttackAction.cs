using System;

namespace RogueLike
{
	public class AttackAction : Action
	{
		Level Level;
		Actor Actor;
		Actor Target;

		public AttackAction (Level aLevel,Actor aActor,Actor aTarget)
		{
			this.Level = aLevel;
			this.Actor = aActor;
			this.Target = aTarget;
		}

		public override ActionResult Perform(){
			ActionResult lResult = new ActionResult(false);

			if (this.Actor.Group == this.Target.Group) {
				lResult.Failure ();
			} else {
				lResult.Success ();
				if (this.Target.TakeDamage (this.Actor.Strength)) {
					this.Level.removeActor (this.Target);
				}
			}

			this.Level.History.AddString(this.Actor.Name + " Attacks " + this.Target.Name);

			return lResult;
		}
	}
}


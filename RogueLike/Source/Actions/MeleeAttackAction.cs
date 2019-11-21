using System;

namespace RogueLike
{
	public class MeleeAttackAction : Action
	{
		Level Level;
		Actor Actor;
		Actor Target;

		public MeleeAttackAction (Actor aActor,Actor aTarget)
		{
			Dungeon lDungeon = Dungeon.Instance;
			this.Level = lDungeon.GetCurrentLevel();
			this.Actor = aActor;
			this.Target = aTarget;
		}

		public override ActionResult Perform(){
			ActionResult lResult = new ActionResult(false);

			if (this.Actor.Group == this.Target.Group) {
				lResult.Failure ();
			}else if (!this.Actor.MeleeRollToHit(this.Target)) {
				lResult.Failure ();
				this.Level.History.AddString (this.Actor.Name + " Misses " + this.Target.Name );
			} else {
				lResult.Success ();
				int lDamage = this.Actor.MeleeDamage ();
				if (this.Target.TakeDamage (lDamage)) {
					this.Level.removeActor (this.Target);
					this.Level.History.AddString (this.Target.Name + " the " + this.Target.Stats.Name + " is dead.");
				} else {
					this.Level.History.AddString (this.Target.Name + " takes " + lDamage.ToString());
				}
			}
				
			return lResult;
		}
	}
}


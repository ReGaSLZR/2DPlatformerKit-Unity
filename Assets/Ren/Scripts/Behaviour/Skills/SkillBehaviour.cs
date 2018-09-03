using UnityEngine;

public abstract class SkillBehaviour : MonoBehaviour 
{
	//NOTE: unfortunately, this produces unforeseen bugs (as tested on CamouflageSkill) and so, the following variables
	//have been commented out. Get back to this and fix the errors when cancelling a skill on 2nd invoke is needed.
	// - Ren

	//determines if this behaviour should be "stopped" when it is invoked while its isInUse value is already TRUE
//	[SerializeField] private bool noDoubleInvoke;
	public bool isInUse {protected set; get;}

	public void UseSkill(bool isRepeating) {
//		if(noDoubleInvoke && isInUse) {
//			LogUtil.PrintWarning(gameObject, GetType(), "No DOUBLE INVOKE at work");
//			UndoSkill();
//			return;
//		}

		isInUse = true;

		if(isRepeating) {
			UseSkillRepeat();	
		} else {
			UseSkillOnce();
		}
	}

	public void UndoSkill() {
		StopSkill();
		isInUse = false;
	}

	protected abstract void UseSkillOnce();
	protected abstract void UseSkillRepeat();

	protected abstract void StopSkill();

}

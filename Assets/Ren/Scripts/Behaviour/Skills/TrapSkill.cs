using System.Collections;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

/*
	NOTE: This skill can only affect the 1st target detected by the TargetDetector.
*/
public class TrapSkill : SkillBehaviour
{

	[SerializeField] private TargetDetector targetDetector;
	[SerializeField] private float delaySkillStop;

	[Header("Is Single-Use Trap?")]
	[SerializeField] private bool isOneTime;
	[Range(0, 2)]
	[SerializeField] private float delayDestroy;
	[SerializeField] private GameObject prefabDestroyFX;

	[Inject] Instantiator instantiator;

	private KillableBehaviour killableTarget;

	private void Awake()
	{
		if(targetDetector == null) {
			LogUtil.PrintWarning(gameObject, GetType(), "TargetDetector is missing.");
			Destroy(this.gameObject);
		}
	}

	protected override void UseSkillOnce()
	{
		StopAllCoroutines();
		StartCoroutine(CorDisableTarget(false));
	}

	protected override void UseSkillRepeat()
	{
		StopAllCoroutines();
		StartCoroutine(CorDisableTarget(true));
	}

	protected override void StopSkill() {
		StopAllCoroutines();
		EnableTarget();

		killableTarget = null;
	}

	private IEnumerator CorDisableTarget(bool toRepeat) {
		if(killableTarget == null) {
			killableTarget = targetDetector.targets[0].gameObject.GetComponent<KillableBehaviour>();
		}
			
		killableTarget.DisableMovement();
		yield return null;

		if(!toRepeat) {
			yield return new WaitForSeconds(delaySkillStop);
			EnableTarget();
		}

		if(isOneTime && (prefabDestroyFX != null)) {
			yield return new WaitForSeconds(delayDestroy);

			instantiator.InjectPrefab(prefabDestroyFX, this.gameObject);
			Destroy(this.gameObject);
		}
	}

	private void EnableTarget() {
		if(killableTarget != null) {
			killableTarget.EnableMovement();
		}
	}

}
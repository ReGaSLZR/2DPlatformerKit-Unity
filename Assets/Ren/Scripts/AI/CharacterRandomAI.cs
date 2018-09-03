using UnityEngine;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterRandomAI : AIBehaviour {

	[SerializeField] PatrolBehaviour[] patrolBehaviours;
	[SerializeField] SkillBehaviour[] skillBehaviours;
	[SerializeField] TargetDetector targetDetector;

	[SerializeField] private bool toRepeatKill;
	[SerializeField] private bool canFaceTarget;

	private SpriteRenderer _spriteRenderer;

	private void Awake() {
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Start() {
		if(targetDetector != null) {
			targetDetector.isTargetDetected
				.Subscribe(isDetected => {
					if(isDetected) {
						FaceTarget();
						UseSkill();
					} else {
						Patrol();
					}
				})
				.AddTo(this);
		}
		else {
			LogUtil.PrintWarning(gameObject, GetType(), "No TargetDetector defined. Reverting to Patrol.");
			Patrol();
		}
	}

	private void FaceTarget() {
		if(canFaceTarget) {
			float targetPosition = targetDetector.targets[0].gameObject.transform.position.x;
			_spriteRenderer.flipX = (targetPosition < this.gameObject.transform.position.x);
		}
	}

	private void Patrol() {
		StopAllSkillBehaviours();
		StartRandomPatrolBehaviour();
	}

	private void UseSkill() {
		StopAllPatrolBehaviours();
		StartRandomSkillBehaviour();
	}

	private void StartRandomSkillBehaviour() {
		int index = Random.Range(0, patrolBehaviours.Length);

		if(skillBehaviours.Length > 0) {
			skillBehaviours[index].UseSkill(toRepeatKill);
		}
	}

	private void StopAllSkillBehaviours() {
		if(skillBehaviours.Length > 0) {
			for(int x=0; x<skillBehaviours.Length; x++) {
				skillBehaviours[x].UndoSkill();
			}
		}
	}

	private void StartRandomPatrolBehaviour() {
		if(patrolBehaviours.Length > 0) {
			patrolBehaviours[Random.Range(0, patrolBehaviours.Length)].StartPatrol();
		}
	}

	private void StopAllPatrolBehaviours() {
		if(patrolBehaviours.Length > 0) {
			for(int x=0; x<patrolBehaviours.Length; x++) {
				patrolBehaviours[x].StopPatrol();
			}
		}
	}

}

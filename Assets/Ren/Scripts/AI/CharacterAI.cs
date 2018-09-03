using UnityEngine;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterAI : AIBehaviour {

	[SerializeField] PatrolBehaviour patrolBehaviour;
	[SerializeField] SkillBehaviour skillBehaviour;
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
		if(canFaceTarget && (targetDetector != null)) {
			float targetPosition = targetDetector.targets[0].gameObject.transform.position.x;
			_spriteRenderer.flipX = (targetPosition < this.gameObject.transform.position.x);
		}
	}

	private void Patrol() {
		StopSkillBehaviour();
		StartPatrolBehaviour();
	}

	private void UseSkill() {
		StopPatrolBehaviour();
		StartSkillBehaviour();
	}

	private void StartSkillBehaviour() {
		if(skillBehaviour != null) {
			skillBehaviour.UseSkill(toRepeatKill);
		}
	}

	private void StopSkillBehaviour() {		
		if(skillBehaviour != null) {
			skillBehaviour.UndoSkill();
		}
	}
		
	private void StartPatrolBehaviour() {		
		if(patrolBehaviour != null) {
			patrolBehaviour.StartPatrol();
		}
	}

	private void StopPatrolBehaviour() {
		if(patrolBehaviour != null) {
			patrolBehaviour.StopPatrol();
		}
	}

}

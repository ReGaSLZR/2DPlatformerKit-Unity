using System.Collections;
using UnityEngine;

/*
	NOTE: This Skill can be used to create a "bounce" effect to its targets. It can also be used for the "hover" effect.
		  All you need to do is tweak the animation. It's named "push off" because "bounce" and "hover" essentially
		  pushes off the other object going against it.
*/

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PushOffSkill : SkillBehaviour
{

	[SerializeField] private TargetDetector detector;

	[Header("Force Push")]
	[SerializeField] private DIRECTION pushDirection = DIRECTION.UP;
	[SerializeField] private float pushOffDelay = 0.1f;
	[SerializeField] private float pushForce = 100f;
	[SerializeField] private string animPushOffTrigger;
	private int animPushOffTriggerId;
	[SerializeField] private string animPushOffBoolStopper;

	[Header("Option to Hurt Target (AKA SpotHit)")]
	[SerializeField] private bool shouldHurtTarget;
	[SerializeField] private bool shouldInstaKillTarget;
	[SerializeField] private AudioClip clipAttack;

	private Animator _animator;
	private AudioSource _audioSource;

	private Vector2 direction;

	private void Awake() {
		_animator = GetComponent<Animator>();
		_audioSource = GetComponent<AudioSource>();

		if(StringUtil.IsNonNullNonEmpty(animPushOffTrigger)) {
			animPushOffTriggerId = Animator.StringToHash(animPushOffTrigger);
		}

		if(detector == null) {
			LogUtil.PrintError(gameObject, GetType(), "Missing TargetDetector. Destroying self... :(");
			Destroy(this.gameObject);
		}

		direction = DirectionUtil.getVectorDirection(pushDirection);
	}

	protected override void UseSkillOnce() {
		StopAllCoroutines(); //for safe measure
		StartCoroutine(CorExecutePushOff(false));
	}

	protected override void UseSkillRepeat() {
		StopAllCoroutines(); //for safe measure
		StartCoroutine(CorExecutePushOff(true));	
	}

	private IEnumerator CorExecutePushOff(bool toRepeat) {
		do {
			StartAnimationPush();

			foreach (Collider2D collider in detector.targets) {
				ApplyPushOff(collider);
				ApplySpotHit(collider);
			}

			yield return new WaitForSeconds(pushOffDelay);

		}while(toRepeat);

		StopAnimationPush();
	}

	private void StartAnimationPush() {
		if(StringUtil.IsNonNullNonEmpty(animPushOffTrigger)) {
			_animator.SetTrigger(animPushOffTriggerId);
		}

		_animator.SetBool(animPushOffBoolStopper, true);
	}

	private void StopAnimationPush() {
		_animator.SetBool(animPushOffBoolStopper, false);
	}

	private void ApplyPushOff(Collider2D collider) {
		Rigidbody2D colliderRigidBody2D = collider.GetComponent<Rigidbody2D>();

		if(colliderRigidBody2D != null) {
			colliderRigidBody2D.AddForce(direction * pushForce);	
		}
	}

	private void ApplySpotHit(Collider2D collider) {
		if(shouldHurtTarget) {
			KillableBehaviour killable = collider.GetComponent<KillableBehaviour>();

			if(killable != null) {
				if(shouldInstaKillTarget) {
					killable.KillOnce();
				} else {
					killable.HitOnce();
				}
			}

			AudioUtil.PlaySingleClip(GetType(), clipAttack, _audioSource);
		}
	}

	protected override void StopSkill() {
		StopAllCoroutines();
		StopAnimationPush();
	}

}


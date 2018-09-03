using System.Collections;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DashSkill : SkillBehaviour
{

	[SerializeField] private float dashForce = 100f;
	[SerializeField] private float skillEffectivityTimer = 1f;
	[SerializeField] private float cooldownTimer = 10f;
	[SerializeField] private AudioClip clipDash;
	[SerializeField] private Transform childDashFX;

	[Header("Animation Parameters")]
	[SerializeField] private string animDashTrigger;
	private int animDashTriggerId;
	[SerializeField] private string animDashStopper;

	[Header("Option to Camouflage")]
	[SerializeField] private bool shouldCamouflage;
	[SerializeField] private OBJECT_TAG tagOnCamouflage = OBJECT_TAG.Player_Camouflaged;

	private Animator _animator;
	private AudioSource _audioSource;
	private Rigidbody2D _rigidBody2D;
	private SpriteRenderer _spriteRenderer;

	private RigidbodyConstraints2D _originalRigidBodyconstraints;
	private RigidbodyConstraints2D dashFreezeConstraints;

	private bool isAvailable = true;
	private string originalTag;

	private void Awake() {
		_animator = GetComponent<Animator>();
		_audioSource = GetComponent<AudioSource>();
		_rigidBody2D = GetComponent<Rigidbody2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();

		_originalRigidBodyconstraints = _rigidBody2D.constraints;
		dashFreezeConstraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

		if(StringUtil.IsNonNullNonEmpty(animDashTrigger)) {
			animDashTriggerId = Animator.StringToHash(animDashTrigger);
		}

		originalTag = this.gameObject.tag;

		TriggerSpotHit(false);
	}

	protected override void UseSkillOnce() {
		if(isAvailable) {
			StartCoroutine(CorExecuteDash());
		}
	}

	protected override void UseSkillRepeat() {
		LogUtil.PrintInfo(gameObject, GetType(), "Sorry, no Repeat mode for DashSkill yet. Invoking UseSkill ONCE instead.");
		UseSkillOnce();
	}

	protected override void StopSkill() {
		if(isInUse) {
			StopAllCoroutines();
			StartCoroutine(CorStopDash());
		}
	}

	private IEnumerator CorExecuteDash() {
		AnimateDash();
		AudioUtil.PlaySingleClip(GetType(), clipDash, _audioSource);

		isAvailable = false;
		_rigidBody2D.AddForce(GetDirection() * dashForce);
		_rigidBody2D.constraints = dashFreezeConstraints;

		TriggerSpotHit(true);
		UseCamouflage();

		yield return new WaitForSeconds(skillEffectivityTimer);
		StartCoroutine(CorStopDash());
	}

	private IEnumerator CorStopDash() {
		StopAnimateDash();

		_rigidBody2D.constraints = _originalRigidBodyconstraints;
		TriggerSpotHit(false);
		UndoCamouflage();

		yield return new WaitForSeconds(cooldownTimer);
		isAvailable = true;
		isInUse = false;
	}

	private void TriggerSpotHit(bool toActivate) {
		if(childDashFX != null) {
			childDashFX.gameObject.SetActive(toActivate);
		}
	}

	private void AnimateDash() {
		if(StringUtil.IsNonNullNonEmpty(animDashTrigger)) {
			_animator.SetTrigger(animDashTriggerId);
		}

		if(StringUtil.IsNonNullNonEmpty(animDashStopper)) {
			_animator.SetBool(animDashStopper, true);
		}
	}

	private void StopAnimateDash() {
		if(StringUtil.IsNonNullNonEmpty(animDashStopper)) {
			_animator.SetBool(animDashStopper, false);
		}
	}

	private void UndoCamouflage() {
		if(shouldCamouflage) {
			this.gameObject.tag = originalTag;
		}
	}

	private void UseCamouflage() {
		if(shouldCamouflage) {
			this.gameObject.tag = tagOnCamouflage.ToString();
		}
	}

	private Vector2 GetDirection() {
		return (_spriteRenderer.flipX) ? Vector2.left : Vector2.right;
	}

}


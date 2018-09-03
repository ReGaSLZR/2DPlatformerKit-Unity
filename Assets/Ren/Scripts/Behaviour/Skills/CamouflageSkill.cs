using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class CamouflageSkill : SkillBehaviour
{

	[SerializeField] private string animTrigger;
	[Tooltip("This is a safety animation Boolean trigger to not allow 'Any State' from firing animCamouflageMain again.")]
	[SerializeField] private string animStopper;
	[SerializeField] private OBJECT_TAG tagOnCamouflage = OBJECT_TAG.Player_Camouflaged;

	[Space]

	[SerializeField] private float delayCamouflage;
	[SerializeField] private float skillEffectivityTimer = 7f;
	[SerializeField] private float cooldownTimer = 14f;
	[SerializeField] private AudioClip clipCamouflage;

	private bool isAvailable = true;
	private string originalTag;

	private Animator _animator;
	private AudioSource _audioSource;

	private int animTriggerId;

	private void Awake() {
		_animator = GetComponent<Animator>();
		_audioSource = GetComponent<AudioSource>();

		if(StringUtil.IsNonNullNonEmpty(animTrigger)) {
			animTriggerId = Animator.StringToHash(animTrigger);	
		}

		originalTag = gameObject.tag;
	}

	private void Start() {

	}

	protected override void UseSkillOnce() {
		ExecuteCamouflage();
	}

	protected override void UseSkillRepeat() {
		LogUtil.PrintInfo(gameObject, GetType(), "Sorry, there are no REPEAT modes for Camouflage. Using Skill ONCE.");
		UseSkillOnce();
	}

	protected override void StopSkill() {
		if(isInUse) {
			StopAllCoroutines();
			UndoCamouflage();
		}
	}

	private void UseCamouflage() {
		if(StringUtil.IsNonNullNonEmpty(animTrigger)) {
			_animator.SetTrigger(animTriggerId);

			if(StringUtil.IsNonNullNonEmpty(animStopper)) {
				_animator.SetBool(animStopper, true);
			}
		}

		AudioUtil.PlaySingleClip(GetType(), clipCamouflage, _audioSource);

		isAvailable = false;
		gameObject.tag = tagOnCamouflage.ToString();
	}

	private void UndoCamouflage() {
		if(StringUtil.IsNonNullNonEmpty(animStopper)) {
			_animator.SetBool(animStopper, false);
		}
			
		isInUse = false;
		gameObject.tag = originalTag;
		StartCoroutine(CorStartCooldownTimer());
	}

	private void ExecuteCamouflage() {
		if(isAvailable) {
			UseCamouflage();
			StartCoroutine(CorStartEffectivityTimer());
		}
		else {
			LogUtil.PrintInfo(gameObject, GetType(), "Camouflage is already in use / still in cooldown.");
		}
	}

	private IEnumerator CorStartEffectivityTimer() {
		LogUtil.PrintInfo(gameObject, GetType(), "Effectivity Timer started");

		yield return new WaitForSeconds(skillEffectivityTimer);
		UndoCamouflage();
	}

	private IEnumerator CorStartCooldownTimer() {
		LogUtil.PrintInfo(gameObject, GetType(), "Cooldown Timer started");

		yield return new WaitForSeconds(cooldownTimer);
		isAvailable = true;
	}

}


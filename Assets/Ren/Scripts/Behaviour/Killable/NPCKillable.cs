using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class NPCKillable : KillableBehaviour {

	[SerializeField] private int hitCount = 1;

	[Header("When Hit")]
	[SerializeField] private string animationHitTrigger;
	private int animationHitTriggerId;
	[SerializeField] private string animationHitStopper;
	[SerializeField] private AudioClip[] clipsHit;
	[SerializeField] private float delayHit;

	[Header("When Killed")]
	[SerializeField] private string animationDeath;
	[SerializeField] private AudioClip[] clipsDeath;
	[SerializeField] private float delayDeath;
	[SerializeField] private GameObject prefabDeathSubstitute;

	[Header("On Death Spawnables")]
	[SerializeField] private SpawnableObject[] spawnObjects;

	[Inject] Instantiator instantiator;

	private Animator _animator;
	private AudioSource _audioSource;
	private Rigidbody2D _rigidBody2D;

	private RigidbodyType2D _rigidBody_type;

	private void Awake() {
		_animator = GetComponent<Animator>();
		_audioSource = GetComponent<AudioSource>();
		_rigidBody2D = GetComponent<Rigidbody2D>();

		if(StringUtil.IsNonNullNonEmpty(animationHitTrigger)) {
			animationHitTriggerId = Animator.StringToHash(animationHitTrigger);
		}

		_rigidBody_type = _rigidBody2D.bodyType;
	}

	public override void DisableMovement() {
		_rigidBody2D.bodyType = RigidbodyType2D.Static;
	}

	public override void EnableMovement() {
		_rigidBody2D.bodyType = _rigidBody_type;
	}

	public override void HitOnce() {
		hitCount--;

		if(hitCount <= 0) {
			KillOnce();
		}
		else {
			StopAllCoroutines();
			StartCoroutine(CorApplyDelayedAnimation(animationHitStopper, clipsHit, delayHit));
		}
	}

	public override void KillOnce() {
		StopAllCoroutines();
		StartCoroutine(CorApplyDelayedAnimation(animationDeath, clipsDeath, delayDeath));

		StartCoroutine(CorDelaySpawnOnDeath());
	}

	private void PlayAnimation(string anim, AudioClip[] clips) {
		_animator.SetTrigger(animationHitTriggerId);

		if(StringUtil.IsNonNullNonEmpty(anim)) {
			_animator.SetBool(anim, true);
		}

		AudioUtil.PlayRandomClip(GetType(), clips, _audioSource);
	}

	private void StopAnimation(string anim) {
		if(StringUtil.IsNonNullNonEmpty(anim)) {
			_animator.SetBool(anim, false);
		}
	}

	private IEnumerator CorApplyDelayedAnimation(string anim, AudioClip[] clips, float delay) {
		PlayAnimation(anim, clips);
		yield return new WaitForSeconds(delay);
		StopAnimation(anim);
	}

	private IEnumerator CorDelaySpawnOnDeath() {
		yield return new WaitForSeconds(delayDeath);

		if(prefabDeathSubstitute != null) {
			instantiator.InjectPrefab(Instantiate(prefabDeathSubstitute, gameObject.transform.position,
				gameObject.transform.rotation));
		}

		SpawnUtil.Spawn(spawnObjects, instantiator, this.gameObject);

		Destroy(this.gameObject);
	}

}

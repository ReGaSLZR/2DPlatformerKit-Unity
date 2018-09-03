using System.Collections;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerKillable : KillableBehaviour
{

	[SerializeField] private string animationFreezeMovement;
	[SerializeField] private AudioClip[] clipsFreeze;

	[Space]
	//NOTE: the usage of Animation Triggers on "Any State" animation transition is for one-time execution of those transitions
	[SerializeField] private string animTrigger; 
	private int animTriggerId;
	//NOTE: the usage of boolean Animation Parameter is for properly ending the animation state at any given moment
	[Tooltip("Boolean animation parameter")]
	[SerializeField] private string animStopper;
	[SerializeField] private AudioClip[] clipsHit;

	[Space]

	[SerializeField] private string animDeath;
	[SerializeField] private AudioClip[] clipsDead;
	[SerializeField] private float delayDestroyOnDeath = 1f;

	private Animator _animator;
	private AudioSource _audioSource;
	private Rigidbody2D _rigidbody2D;

	[Inject] PlayerStats_Observer playerStats;
	[Inject] PlayerStatSetter_Health health;
	[Inject] PlayerStatSetter_Lives lives;

	private void Awake() {
		_animator = GetComponent<Animator>();
		_audioSource = GetComponent<AudioSource>();
		_rigidbody2D = GetComponent<Rigidbody2D>();

		if(StringUtil.IsNonNullNonEmpty(animTrigger)) {
			animTriggerId = Animator.StringToHash(animTrigger);
		}
	}

	private void Start() {
		playerStats.IsHit()
			.Subscribe(isHit => ApplyHit(isHit))
			.AddTo(this);

		playerStats.IsFinalHit()
			.Where(isFinalHit => isFinalHit)
			.Subscribe(_ => {
				StopAllCoroutines();
				StartCoroutine(CorApplyDeath());
			})
			.AddTo(this);
	}

	private void ApplyHit(bool isHit) {
		if(!playerStats.IsFinalHit().Value) { //ignore Hit animation if Player has been hit for the FINAL time (gameOver)
			if(isHit) {
				_animator.SetTrigger(animTriggerId);
			}
				
			_animator.SetBool(animStopper, isHit);

			if(isHit) {
				AudioUtil.PlayRandomClip(GetType(), clipsHit, _audioSource);
			}
		}
	}

	private IEnumerator CorApplyDeath() {
		_animator.SetBool(animStopper, false);
		_audioSource.Stop();

		_animator.SetBool(animDeath, true);
		AudioUtil.PlayRandomClip(GetType(), clipsDead, _audioSource);

		yield return new WaitForSeconds(delayDestroyOnDeath);
		Destroy(this.gameObject);
	}

	public override void DisableMovement() {
		_rigidbody2D.bodyType = RigidbodyType2D.Static;

		if(StringUtil.IsNonNullNonEmpty(animationFreezeMovement)) {
			_animator.SetBool(animationFreezeMovement, true);
			AudioUtil.PlayRandomClip(GetType(), clipsFreeze, _audioSource);
		}
	}

	public override void EnableMovement() {
		_rigidbody2D.bodyType = RigidbodyType2D.Dynamic;

		if(StringUtil.IsNonNullNonEmpty(animationFreezeMovement)) {
			_animator.SetBool(animationFreezeMovement, false);
		}
	}

	public override void HitOnce() {
		health.HitOnce();
	}

	public override void KillOnce() {
		lives.KillOnce();
	}

}


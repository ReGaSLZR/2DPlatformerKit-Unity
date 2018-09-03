using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]

public class SpotHitSkill : SkillBehaviour 
{

	[SerializeField] private TargetDetector detector;
	[SerializeField] private bool instaKill;
	[Header("Option to Destroy")]
	[SerializeField] private bool destroyOnKill;
	[SerializeField] private GameObject prefabDestroySub;
	[Space]
	[SerializeField] private float delayStart = 0.1f;
	[SerializeField] private float delayEnd = 0.3f;
	[SerializeField] private AudioClip clipAttack;
	[SerializeField] private string animationAttack;

	private Animator _animator;
	private AudioSource _audioSource;

	[Inject] Instantiator _instantiator;

	private void Awake() {
		if(detector == null) {
			LogUtil.PrintError(gameObject, GetType(), "Missing TargetDetector. Destroying self... :(");
			Destroy(this.gameObject);
		}

		_animator = GetComponent<Animator>();
		_audioSource = GetComponent<AudioSource>();
	}

	protected override void UseSkillOnce() {
		StopSkill();
		StartCoroutine(CorKill(false));
	}

	protected override void UseSkillRepeat() {
		StopSkill();
		StartCoroutine(CorKill(true));
	}

	protected override void StopSkill() {
		SafelyPlayKillAnimation(false);
		StopAllCoroutines();
	}

	private void OnDisable() {
		StopSkill();
	}

	private IEnumerator CorKill(bool toRepeat) {
		do {
			SafelyPlayKillAnimation(true);

			yield return new WaitForSeconds(delayStart);

			PlayClipAttack();
			ExecuteKill();
			SafelyPlayKillAnimation(false);

			yield return new WaitForSeconds(delayEnd);

			if(destroyOnKill) {
				_instantiator.InjectPrefab(prefabDestroySub, this.gameObject);
				Destroy(this.gameObject);
			}
		} while(toRepeat);
	}

	private void PlayClipAttack() {
		if(clipAttack != null) {
			_audioSource.clip = clipAttack;
			_audioSource.Play();
		} else {
			LogUtil.PrintWarning(this.gameObject, this.GetType(), "Not calling PlayClipAttack(). No AudioClip defined.");
		}
	}

	private void SafelyPlayKillAnimation(bool toPlay) {
		if(StringUtil.IsNonNullNonEmpty(animationAttack)) {
			_animator.SetBool(animationAttack, toPlay);
		}
	}

	private void ExecuteKill() {
		if(instaKill) {
			KillAll(GetKillables());
		}
		else {
			HitAll(GetKillables());
		}
	}

	private List<KillableBehaviour> GetKillables() {
		List<KillableBehaviour> killables = new List<KillableBehaviour>();

		for(int x=0; x<detector.targets.Count; x++) {
			if(detector.targets[x] != null) {
				KillableBehaviour killable = detector.targets[x].gameObject.GetComponent<KillableBehaviour>();

				if(killable != null) {
					killables.Add(killable);
				}
			}
		}

		return killables;
	}

	private void KillAll(List<KillableBehaviour> killables) {
		foreach(KillableBehaviour killable in killables) {
			killable.KillOnce();
		}
	}

	private void HitAll(List<KillableBehaviour> killables) {
		foreach(KillableBehaviour killable in killables) {
			killable.HitOnce();
		}
	}
		
}

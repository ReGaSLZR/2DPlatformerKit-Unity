using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class IlludeSkill : SkillBehaviour {

	[SerializeField] private ILLUSION_TYPE illusionType = ILLUSION_TYPE.APPEAR;
	[SerializeField] private OBJECT_TAG tagOnIllude;
	[SerializeField] private AudioClip clipIllusion;
	[SerializeField] private float alphaIlludeInterval = 0.08f;
	[SerializeField] private float frameInterval = 0.03f;

	private AudioSource _audioSource;
	private Collider2D _collider2D;
	private SpriteRenderer _spriteRenderer;

	private string originalTag;

	private void Awake() {
		_audioSource = GetComponent<AudioSource>();
		_collider2D = GetComponent<Collider2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();

		originalTag = gameObject.tag;
	}

	private void Start() {
		InitializePreIlludeState();
	}

	protected override void UseSkillOnce() {
		ExecuteIllusion();
	}

	protected override void UseSkillRepeat() {
		LogUtil.PrintInfo(gameObject, GetType(), "Sorry, there is NO repeat mode for Illusion Skill.");
		UseSkillOnce();
	}

	protected override void StopSkill() {
		if(isInUse) {
			StopAllCoroutines();
			InitializePreIlludeState();
			gameObject.tag = originalTag;
		}
	}

	private void InitializePreIlludeState() {
		Color tempColor = _spriteRenderer.color;

		switch(illusionType) {
			case ILLUSION_TYPE.APPEAR : {
				tempColor.a = 0f;
				ChangeColliderTrigger(true);
				break;
			}
			case ILLUSION_TYPE.DISAPPEAR : {
				tempColor.a = 1f;
				ChangeColliderTrigger(false);
				break;
			}
		}

		_spriteRenderer.color = tempColor;
	}

	private void ChangeColliderTrigger(bool isTrigger) {
		_collider2D.isTrigger = isTrigger;
	}

	private IEnumerator CorAppear() {
		do {
			Color tempColor = _spriteRenderer.color;
			tempColor.a += alphaIlludeInterval;

			_spriteRenderer.color = tempColor;
			yield return new WaitForSeconds(frameInterval);

		} while(_spriteRenderer.color.a < 1f);	

		ChangeColliderTrigger(false);
	}

	private IEnumerator CorDisappear() {
		do {
			Color tempColor = _spriteRenderer.color;
			tempColor.a -= alphaIlludeInterval;

			_spriteRenderer.color = tempColor;
			yield return new WaitForSeconds(frameInterval);

		} while(_spriteRenderer.color.a > 0f);	

		ChangeColliderTrigger(true);
	}

	private void ExecuteIllusion() {
		AudioUtil.PlaySingleClip(GetType(), clipIllusion, _audioSource);

		switch(illusionType) {
			case ILLUSION_TYPE.APPEAR : {
				StartCoroutine(CorAppear());
				break;
			}
			case ILLUSION_TYPE.DISAPPEAR : {
				StartCoroutine(CorDisappear());
				break;
			}
		}

		gameObject.tag = tagOnIllude.ToString();
	}

}

public enum ILLUSION_TYPE {
	APPEAR, DISAPPEAR
}

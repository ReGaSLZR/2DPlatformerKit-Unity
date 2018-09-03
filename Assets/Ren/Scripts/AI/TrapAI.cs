using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

/*
	NOTE: This script shall be removed in the next iteration. Do NOT use this anymore as much as possible. 
		  To achieve similar behaviour, please use the combo of CharacterAI+PushOffSkill       -Ren
*/

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
public class TrapAI : AIBehaviour
{

	[SerializeField] private int lives = 1;
	[SerializeField] private float selfDestroyDelay = 1f;
	[SerializeField] private GameObject prefabDestroyFX;

	[Header("Insta Kill or Take 1 Heart/Hit?")]
	[SerializeField] private bool instaKill;

	[Header("Hit Impact")]
	[SerializeField] private bool shouldApplyImpact;
	[SerializeField] private bool camouflageCheck;
	[SerializeField] private float impactValue;
	[SerializeField] private DIRECTION impactDirection;

	[Inject] PlayerStatSetter_Health playerHealth;
	[Inject] PlayerStatSetter_Lives playerLives;

	[Inject] Instantiator instantiator;

	private AudioSource _audioSource;

	private void Awake()
	{
		_audioSource = this.GetComponent<AudioSource>();
	}

	private void Start()
	{
		this.OnCollisionEnter2DAsObservable()
			.Where(collision2D => TagUtil.IsTagPlayer(collision2D.gameObject.tag, camouflageCheck))
			.Subscribe(collision2D => { 
				ApplyForce(collision2D);
				ApplyDamage();
				DeductLife();
				PlaySFX();
			})
			.AddTo(this);
	}

	private void ApplyForce(Collision2D collision2D) {
		if(shouldApplyImpact) {
			Rigidbody2D playerBody = collision2D.gameObject.GetComponent<Rigidbody2D>();
			playerBody.AddForce(DirectionUtil.getVectorDirection(impactDirection) * impactValue);
		}
	}

	private void ApplyDamage()
	{
		if(instaKill) {
			playerLives.KillOnce();
		}
		else {
			playerHealth.HitOnce();
		}
	}

	private void PlaySFX() {
		if(_audioSource.isPlaying) {
			_audioSource.Stop();
		}

		_audioSource.Play();
	}

	private void DeductLife() {
		if(lives <= 1) {
			if(prefabDestroyFX != null) {
				instantiator.InjectPrefab(Instantiate(prefabDestroyFX, this.gameObject.transform.position,
					this.gameObject.transform.rotation));
			}

			Destroy(this.gameObject, selfDestroyDelay);
		} else {
			lives--;
		}
	}
	

}


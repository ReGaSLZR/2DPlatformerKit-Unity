using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]

public class ThrowSkill : SkillBehaviour 
{

	[SerializeField] protected GameObject prefabProjectile;
	[SerializeField] protected AudioClip[] clipsThrow;
	[SerializeField] protected string animationParam;

	[SerializeField] private float force = 100f;
	[SerializeField] private float torque = 50f;
	[SerializeField] private float resetDelayStart = 0.1f;
	[SerializeField] private float resetDelayEnd = 0.5f;

	[Header("Spawners")]
	[SerializeField] private GameObject spawnerRight;
	[SerializeField] private GameObject spawnerLeft;

	[Inject] Instantiator instantiator;

	//COMPONENTS
	private Animator _animator;
	private AudioSource _audioSource;
	private SpriteRenderer _spriteRenderer;

	private bool isSkillInPlay = false;

	protected override void UseSkillOnce() {
		if(!isSkillInPlay) {
			StartCoroutine(CorExecute(false));
		}
	}

	protected override void UseSkillRepeat() {
		if(!isSkillInPlay) {
			StartCoroutine(CorExecute(true));
		}
	}

	protected override void StopSkill() {
		TriggerAnimation(false);	
		StopAllCoroutines();
		isSkillInPlay = false;
	}

	private void Awake() {
		_animator = this.GetComponent<Animator>();
		_audioSource = this.GetComponent<AudioSource>();
		_spriteRenderer = this.GetComponent<SpriteRenderer>();
	}

	private IEnumerator CorExecute(bool onRepeat) {
		do {
			isSkillInPlay = true;
			TriggerAnimation(true);
			yield return new WaitForSeconds(resetDelayStart);
		
			FireProjectile();
			AudioUtil.PlayRandomClip(GetType(), clipsThrow, _audioSource);

			yield return new WaitForSeconds(resetDelayEnd);
			TriggerAnimation(false);
			isSkillInPlay = false;

			LogUtil.PrintInfo(gameObject, GetType(), "CorExecute() onRepeat? " + onRepeat);
		} while(onRepeat);
	}

	private void FireProjectile() {
		if(prefabProjectile != null) {
			GameObject spawner = GetProjectileSpawner();

			GameObject projectile = Instantiate(prefabProjectile, 
				spawner.transform.position, spawner.transform.rotation);
			Rigidbody2D rigidBody2DProjectile = projectile.GetComponent<Rigidbody2D>();

			instantiator.InjectPrefab(projectile);
			rigidBody2DProjectile.AddForce(GetDirection() * force);
			rigidBody2DProjectile.AddTorque(torque, ForceMode2D.Force);
		}
	}

	private Vector2 GetDirection() {
		return (_spriteRenderer.flipX) ? Vector2.left : Vector2.right;
	}

	private GameObject GetProjectileSpawner() {
		return (_spriteRenderer.flipX) ? spawnerLeft : spawnerRight;
	}

	private void TriggerAnimation(bool isOn) {
		_animator.SetBool(animationParam, isOn);	
	}
		
}

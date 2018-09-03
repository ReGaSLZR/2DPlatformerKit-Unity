using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class JumpPatrol : PatrolBehaviour
{

	[SerializeField] private float jumpSpeed = 1f;
	[SerializeField] private float jumpHeight = 1f;
	[SerializeField] private float jumpIntervalStart = 1.5f;
	[SerializeField] private float jumpIntervalEnd = 1.5f;
	[SerializeField] private string animationJumpUp;
//	[SerializeField] private string animationJumpLand;
	[SerializeField] private bool defaultFlipXDirection;
	[SerializeField] private bool isChangingFlipX;

	private Animator _animator;
	private Rigidbody2D _rigidbody2D;
	private SpriteRenderer _spriteRenderer;

	private void Awake() {
		_animator = this.GetComponent<Animator>();
		_rigidbody2D = this.GetComponent<Rigidbody2D>();
		_spriteRenderer = this.GetComponent<SpriteRenderer>();
	}

	public override void StartPatrol() {
		_spriteRenderer.flipX = defaultFlipXDirection;

		StopAllCoroutines();
		StartCoroutine(StartJumping(true));
	}

	public override void StopPatrol() {
		StopAllCoroutines();
		_animator.SetBool(animationJumpUp, false);
	}

	private IEnumerator StartJumping(bool isRepeating) {
		while(isRepeating) {
			_animator.SetBool(animationJumpUp, true);
			_rigidbody2D.AddForce(Vector2.up * (jumpSpeed * jumpHeight));

			if(isChangingFlipX) {
				_spriteRenderer.flipX = (Random.Range(0, 1) == 0);
			}

			yield return new WaitForSeconds(jumpIntervalStart);
			_animator.SetBool(animationJumpUp, false);

			yield return new WaitForSeconds(jumpIntervalEnd);
		}
	}

}


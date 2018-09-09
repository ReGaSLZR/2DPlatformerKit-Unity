using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{

	[Header("Horizontal Movement")]
	[SerializeField] private float walkSpeed = 0.1f;

	[Header("Wall Slide")]
	[SerializeField] private float normalFrictionDrag = 1f;
	[SerializeField] private float wallSlideFriction = 10f;
	[SerializeField] private AudioClip wallSlideClip;

	[Header("Jump")]
	[SerializeField] private float jumpSpeed = 1.2f;
	[SerializeField] private float jumpHeight = 250f;
	[SerializeField] private AudioClip[] clipsJump;
	[SerializeField] private GameObject prefabJumpFX;
	[SerializeField] private GameObject originJumpFX;
	[Space]
	[SerializeField] private int jumpTimesMax = 1;
	[SerializeField] private int jumpsLeft = 1;
	[SerializeField] private float jumpInterval = 0.5f;

	[Header("Animation Params")]
	[SerializeField] private string paramIsWalking;
	[SerializeField] private string paramIsGrounded;
	[SerializeField] private string paramIsWallSliding;

	[Inject] PlayerGround_Observer groundObserver;
	[Inject] PlayerInputControls playerInput;
	[Inject] Instantiator instantiator;

	//COMPONENTS
	private Animator animator;
	private AudioSource _audioSource;
	private Rigidbody2D rigidBody2D;
	private SpriteRenderer spriteRenderer;

	private DateTimeOffset _lastJumped;

	private void Awake() {
		animator = GetComponent<Animator>();
		_audioSource = GetComponent<AudioSource>();
		rigidBody2D = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Start() {
		InitObservers();
	}

	private void InitObservers() {
		this.FixedUpdateAsObservable()
			.Where(_ => (playerInput.movement == 0f))
			.Subscribe(_ => AnimateIdleMovement())
			.AddTo(this);

		this.FixedUpdateAsObservable()
			.Select(_ => playerInput.movement)
			.Where(horizontalMovement => (horizontalMovement != 0))
			.Subscribe(horizontalMovement => {
				AnimateHorizontalMovement(horizontalMovement);
				CheckFlipHorizontal(horizontalMovement);
				MoveHorizontally(horizontalMovement);
			})
			.AddTo(this);

		this.FixedUpdateAsObservable()
			.Select(_ => playerInput.hasJumped)
			.Where(hasJumped => (hasJumped && (jumpsLeft > 0)))
			.Timestamp()
			.Where(x => x.Timestamp > _lastJumped.AddSeconds(jumpInterval))
			.Subscribe(x => {
				Jump();
				_lastJumped = x.Timestamp;
			})
			.AddTo(this);

		groundObserver.IsGrounded()
			.Subscribe(isOnGround => AnimateChangeGround(paramIsGrounded, isOnGround))
			.AddTo(this);

		groundObserver.IsWallSliding()
			.Subscribe(isSliding => { 
				AnimateChangeGround(paramIsWallSliding, isSliding);

				if(isSliding && (playerInput.movement == 0)) {
					CheckFlipHorizontal(playerInput.movement);
				}

				ApplyWallSlide(isSliding);
			})
			.AddTo(this);

	}

	private void AnimateIdleMovement() {
		if(animator.GetBool(paramIsWalking)) {
			animator.SetBool(paramIsWalking, false);
		}
	}

	private void AnimateHorizontalMovement(float horizontalMovement) {
		bool isSliding = (horizontalMovement != 0);

		if(isSliding != animator.GetBool(paramIsWalking)) { //to prevent jitter
			animator.SetBool(paramIsWalking, isSliding);
		}
	}

	private void CheckFlipHorizontal(float horizontalMovement) {
		bool shouldFlip = (horizontalMovement < 0f);

		//NOTE: this extra statement may negate the value of 'shouldFlip' if Player is WallSliding
		shouldFlip =  (groundObserver.IsWallSliding().Value) ? 
					  (groundObserver.GetWallSide().Value == WALL_SLIDE_SIDE.RIGHT) 
						: shouldFlip; 

		//condition is to prevent jittering
		//do NOT flip if the previous value is the same as the new one
		if(spriteRenderer.flipX != shouldFlip) { 
			spriteRenderer.flipX = shouldFlip;
		}
	}

	private void MoveHorizontally(float horizontalMovement) {
		Vector2 movement = Vector2.zero;
		movement.x = horizontalMovement;

		rigidBody2D.position = (rigidBody2D.position + 
			(movement * walkSpeed * Time.fixedDeltaTime));
	}

	private void Jump() {
		AudioUtil.PlayRandomClip(GetType(), clipsJump, _audioSource);

		if((prefabJumpFX != null) && (originJumpFX != null)) {
			instantiator.InjectPrefab(Instantiate(prefabJumpFX, originJumpFX.transform.position,
				originJumpFX.transform.rotation));
		}
	
		rigidBody2D.AddForce(Vector2.up * (jumpHeight * jumpSpeed), ForceMode2D.Impulse);
		jumpsLeft--;
	}

	private void AnimateChangeGround(string paramGround, bool isActive) {
		if(animator.GetBool(paramGround) != isActive) {
			animator.SetBool(paramGround, isActive);
		}
			
		if(isActive) {
			if((paramIsGrounded.Equals(paramGround))) {
				_audioSource.Stop();
			}
			else if(paramIsWallSliding.Equals(paramGround) && (!groundObserver.IsGrounded().Value)) {
				AudioUtil.PlaySingleClip(GetType(), wallSlideClip, _audioSource);
			}
		}

		//reset jumps
		if(isActive) { jumpsLeft = jumpTimesMax; }
	}

	private void ApplyWallSlide(bool isActive) {
		rigidBody2D.drag = (isActive) ? wallSlideFriction : normalFrictionDrag;
	}

}

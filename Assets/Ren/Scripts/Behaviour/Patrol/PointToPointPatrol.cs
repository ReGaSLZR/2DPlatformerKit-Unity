using System.Collections;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class PointToPointPatrol : PatrolBehaviour 
{

	[SerializeField] private float patrolSpeed = 1f;
	[SerializeField] private string animationPatrol;

	[SerializeField] private RectTransform.Axis patrolDirection;
	[SerializeField] private bool canTurnAround;

	[Header("Limited to 2 Patrol Points for now")]
	[SerializeField] private float patrolPointLesser; //left or down patrol point
	[SerializeField] private float patrolPointGreater; //right or up patrol point

	private Animator _animator;
	private Rigidbody2D _rigidbody2D;
	private SpriteRenderer _spriteRenderer;

	private bool toPatrol;
	private float currentPatrolPoint;
	private Vector2 direction;

	private void Awake() {
		_animator = this.GetComponent<Animator>();
		_rigidbody2D = this.GetComponent<Rigidbody2D>();
		_spriteRenderer = this.GetComponent<SpriteRenderer>();

		direction = (patrolDirection == RectTransform.Axis.Horizontal) ? Vector2.right : Vector2.up;
	}

	private void Start() {
		this.FixedUpdateAsObservable()
			.Select(_ => toPatrol).Where(go => go)
			.Subscribe(_ => Patrol())
			.AddTo(this);

		this.OnCollisionEnter2DAsObservable()
			.Where(otherCollision2D => otherCollision2D.gameObject.CompareTag(gameObject.tag))
			.Subscribe(_ => ReversePatrol())
			.AddTo(this);
	}

	public override void StartPatrol() {
		_animator.SetBool(animationPatrol, true);
		toPatrol = true;
		currentPatrolPoint = patrolPointLesser;
	}

	public override void StopPatrol() {
		_animator.SetBool(animationPatrol, false);
		toPatrol = false;
	}

	private void Patrol(){
		if ((GetCurrentPosition() > patrolPointLesser) && (currentPatrolPoint == patrolPointLesser)) {
			PatrolLesser();
		}
		else {
			PatrolGreater();
		}
	}

	private void ReversePatrol() {
		currentPatrolPoint = (currentPatrolPoint == patrolPointLesser) ? patrolPointGreater : patrolPointLesser;
	}

	//NOTE: Either goes to the LEFT or DOWN
	private void PatrolLesser() {
		if(canTurnAround && !_spriteRenderer.flipX) { _spriteRenderer.flipX = true; }

		Vector2 vector = Vector2.zero;
		vector = (-1) * direction;

		Move(vector);
		ResetDestinationPoint(GetCurrentPosition(), true, patrolPointLesser, patrolPointGreater);
	}

	//NOTE: Either goes to the RIGHT or UP
	private void PatrolGreater() {
		if(canTurnAround && _spriteRenderer.flipX) { _spriteRenderer.flipX = false; }

		Vector2 vector = Vector2.zero;
		vector = direction;

		Move(vector);
		ResetDestinationPoint(GetCurrentPosition(), false, patrolPointGreater, patrolPointLesser);
	}

	private void Move(Vector2 vector) {
//		_rigidbody2D.position = (_rigidbody2D.position + (vector * patrolSpeed * Time.fixedDeltaTime));
		_rigidbody2D.MovePosition(_rigidbody2D.position + (vector * patrolSpeed * Time.fixedDeltaTime));
	}

	private void ResetDestinationPoint(float currentPosition, bool isLesser, float currentDestination, float newDestination) {
		bool checker = (isLesser) ? (((int) currentPosition) <= ((int) currentDestination)) 
								  : (((int) currentPosition) >= ((int) currentDestination));

		if(checker) {
			currentPatrolPoint = newDestination;
		}
	}

	private float GetCurrentPosition() {
		return (patrolDirection == RectTransform.Axis.Horizontal) ? _rigidbody2D.position.x : _rigidbody2D.position.y;
	}

}

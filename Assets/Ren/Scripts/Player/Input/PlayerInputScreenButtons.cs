using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class PlayerInputScreenButtons : PlayerInputControls {

	[SerializeField] private Button attack;
	[SerializeField] private Button skill1;
	[SerializeField] private Button skill2;
	[Space]
 	[SerializeField] private Button jump;
	[SerializeField] private Button moveRight;
	[SerializeField] private Button moveLeft;
	[Space]
	[SerializeField] private Button pauseGame;

	private void Start() {
		SetUpAttack();
		SetUpSkill1();
		SetUpSkill2();
		SetUpJump();

		SetUpMovement();
//		SetUpPauseControl();
	}

//	private void SetUpPauseControl() {
//		pauseGame.OnClickAsObservable()
//			.Timestamp()
//			.Where(x => x.Timestamp > _lastPaused.AddSeconds(pauseInterval))
//			.Subscribe(x => {
//				hasPaused.Value = !hasPaused.Value; 
//				_lastPaused = x.Timestamp;
//			})
//			.AddTo(this);
//	}

	private void SetUpMovement() {
		moveRight.OnPointerDownAsObservable()
			.Where(_ => IsInputEnabled())
			.Subscribe(_ => movement = movementBaseSpeed)
			.AddTo(this);

		moveLeft.OnPointerDownAsObservable()
			.Where(_ => IsInputEnabled())
			.Subscribe(_ => movement = (movementBaseSpeed * -1))
			.AddTo(this);

		moveRight.OnPointerUpAsObservable()
			.Where(_ => IsInputEnabled())
			.Subscribe(_ => movement = 0f)
			.AddTo(this);

		moveLeft.OnPointerUpAsObservable()
			.Where(_ => IsInputEnabled())
			.Subscribe(_ => movement = 0f)
			.AddTo(this);
	}

	private void SetUpAttack() {
		attack.OnPointerDownAsObservable()
			.Where(_ => IsInputEnabled())
			.Subscribe(_ => {
				hasAttacked = true;
			})
			.AddTo(this);

		attack.OnPointerUpAsObservable()
			.Where(_ => IsInputEnabled())
			.Subscribe(_ => {
				hasAttacked = false;
			})
			.AddTo(this);
	}

	private void SetUpSkill1() {
		skill1.OnPointerDownAsObservable()
			.Where(_ => IsInputEnabled())
			.Subscribe(_ => {
				hasActivated_Skill1 = true;
			})
			.AddTo(this);

		skill1.OnPointerUpAsObservable()
			.Where(_ => IsInputEnabled())
			.Subscribe(_ => {
				hasActivated_Skill1 = false;
			})
			.AddTo(this);
	}

	private void SetUpSkill2() {
		skill2.OnPointerDownAsObservable()
			.Where(_ => IsInputEnabled())
			.Subscribe(_ => {
				hasActivated_Skill2 = true;
			})
			.AddTo(this);

		skill2.OnPointerUpAsObservable()
			.Where(_ => IsInputEnabled())
			.Subscribe(_ => {
				hasActivated_Skill2 = false;
			})
			.AddTo(this);
	}

	private void SetUpJump() {
		jump.OnPointerDownAsObservable()
			.Where(_ => IsInputEnabled())
			.Subscribe(_ => {
				hasJumped = true;
			})
			.AddTo(this);

		jump.OnPointerUpAsObservable()
			.Where(_ => IsInputEnabled())
			.Subscribe(_ => {
				hasJumped = false;
			})
			.AddTo(this);
	}

}

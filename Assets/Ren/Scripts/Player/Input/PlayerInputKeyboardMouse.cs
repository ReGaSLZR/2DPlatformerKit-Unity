using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class PlayerInputKeyboardMouse : PlayerInputControls {

	[SerializeField] private KeyCode attack = KeyCode.J;
	[SerializeField] private KeyCode skill1 = KeyCode.K;
	[SerializeField] private KeyCode skill2 = KeyCode.L;

	[Space]

	[SerializeField] private KeyCode jump = KeyCode.I;
	[SerializeField] private KeyCode moveRight = KeyCode.D;
	[SerializeField] private KeyCode moveLeft = KeyCode.A;

	[Space]

//	[SerializeField] private KeyCode pauseKey = KeyCode.Escape;
//	[SerializeField] private Button[] pauseButtons; 

	private DateTimeOffset pauseTimeStamp;

	private void Start() {
		SetUpMovementControls();
		SetUpSkillControls();

		this.FixedUpdateAsObservable()
			.Where(_ => IsInputEnabled())
			.Select(_ => Input.GetKeyDown(jump))
			.Subscribe(isPressed => hasJumped = isPressed)
			.AddTo(this);

		this.FixedUpdateAsObservable()
			.Where(_ => IsInputEnabled())
			.Select(_ => Input.GetKeyDown(attack) 
//				&& !hasPaused.Value
			)
			.Subscribe(isPressed => hasAttacked = isPressed)
			.AddTo(this);

//		this.FixedUpdateAsObservable()
//			.Select(_ => Input.GetKeyDown(pauseKey))
//			.Where(isPressed => isPressed && !isDisabled)
//			.Timestamp()
//			.Where(x => x.Timestamp > pauseTimeStamp.AddSeconds(0.1))
//			.Subscribe(x => { 
//				hasPaused.Value = !hasPaused.Value;
//				pauseTimeStamp = x.Timestamp;
//			})
//			.AddTo(this);

//		if(pauseButtons == null || pauseButtons.Length == 0) {
//			LogUtil.PrintWarning(gameObject, GetType(), "No pauseButton defined :(");
//		}
//		else {
//			foreach(Button pauseButton in pauseButtons) {
//				pauseButton.OnClickAsObservable()
//					.Timestamp()
//					.Where(x => x.Timestamp > _lastPaused.AddSeconds(pauseInterval))
//					.Subscribe(x => {
//						hasPaused.Value = !hasPaused.Value; 
//						_lastPaused = x.Timestamp;
//					})
//					.AddTo(this);
//			}
//		}
	}

	private void SetUpSkillControls() {
		this.FixedUpdateAsObservable()
			.Where(_ => IsInputEnabled())
			.Select(_ => Input.GetKeyDown(skill1) 
//				&& !hasPaused.Value
			)
			.Subscribe(isPressed => hasActivated_Skill1 = isPressed)
			.AddTo(this);

		this.FixedUpdateAsObservable()
			.Where(_ => IsInputEnabled())
			.Select(_ => Input.GetKeyDown(skill2) 
//				&& !hasPaused.Value
			)
			.Subscribe(isPressed => hasActivated_Skill2 = isPressed)
			.AddTo(this);
	}

	private void SetUpMovementControls() {
		this.FixedUpdateAsObservable()
			.Where(_ => IsInputEnabled())
			.Select(_ => Input.GetKey(moveRight))
			.Where(isPressed => isPressed)
			.Subscribe(isPressed => movement = movementBaseSpeed)
			.AddTo(this);

		this.FixedUpdateAsObservable()
			.Where(_ => IsInputEnabled())
			.Select(_ => Input.GetKey(moveLeft))
			.Where(isPressed => isPressed)
			.Subscribe(isPressed => movement = (movementBaseSpeed * -1))
			.AddTo(this);

		this.FixedUpdateAsObservable()
			.Where(_ => IsInputEnabled())
			.Select(_ => (!Input.GetKey(moveRight) && !Input.GetKey(moveLeft)))
			.Where(isLetGo => isLetGo)
			.Subscribe(_ => movement = 0f)
			.AddTo(this);

	}

}

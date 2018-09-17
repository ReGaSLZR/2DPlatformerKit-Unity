using System;
using UnityEngine;
using UniRx;

public abstract class PlayerInputControls : MonoBehaviour,
										    InputControlDisabler
{

	[SerializeField] protected float movementBaseSpeed = 0.1f;

//	public ReactiveProperty<bool> hasPaused {protected set; get;}
//	protected float pauseInterval = 1f;
//	protected DateTimeOffset _lastPaused;

	public bool hasAttacked {protected set; get;}
	public bool hasActivated_Skill1 {protected set; get;}
	public bool hasActivated_Skill2 {protected set; get;}

	public bool hasJumped {protected set; get;}
	public float movement {protected set; get;}

	protected bool isDisabled;

	public bool hasScreenButtons {protected set; get;}

	private void Awake() {
//		hasPaused = new ReactiveProperty<bool>(false);
		isDisabled = false;
	}

	protected bool IsInputEnabled() {
		return (!isDisabled);
	}

	public void DisableControls() {
		isDisabled = true;
		movement = 0f;
	}

	public void EnableControls() {
		isDisabled = false;
	}

//	public void SetMovement(float newMovement) {
//		movement = newMovement;
//	}

}

/*		INTERFACES		*/
public interface InputControlDisabler {
	void EnableControls();
	void DisableControls();
}

//public interface InputMovement_SpecialSetter {
//	void SetMovement(float newMovement);
//}
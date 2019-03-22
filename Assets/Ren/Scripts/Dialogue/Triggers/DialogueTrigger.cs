using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

/*
	NOTE: Remember to place the trigger somewhere the Player is "safe" from enemy range.
		  This script may disable the Player Input during the dialogue sequence (if 'toDisablePlayerMovement' is TRUE)
		  but does not disable the movement of enemies at all.
	NOTE: If we set Time.timeScale to ZERO, all characters (Player + enemies) will be frozen but so will the feeder. :(
		  Which is why we may only opt to disable the Player via InputControlDisabler, NOT Time.timeScale :)
*/

[RequireComponent(typeof(Collider2D))]
public abstract class DialogueTrigger : MonoBehaviour {

	[SerializeField] private Transform focusedObject;
	[SerializeField] private InGameEvent[] afterEvents;
	[Space]
	[SerializeField] protected bool isOneTime;
	[SerializeField] protected bool toDisablePlayerMovement;
	[SerializeField] protected bool toHalveBGMVolume;

	[Tooltip("Disregard the value of this if the playerMovement is to be disabled")]
	[SerializeField] protected bool toFinishOnTriggerExit;

	[Space]
	[SerializeField] protected string textButtonNext = "Next";
	[SerializeField] protected string textButtonFinish = "Ok";

	[Inject] protected VolumeController_Setter volumeSetter; //TODO decide if this is to be injected or use VolumeController_Setter instead
	[Inject] protected InputControlDisabler playerInput;
	[Inject] protected DialogueController_Observer dialogueGetter;
	[Inject] protected DialogueController_Setter dialogueSetter;

	[Inject] Timer_Setter timerSetter;

	private bool isTriggered = false;
	protected abstract void StartDialogue();

	private void Start() {
		if(toFinishOnTriggerExit) {
			this.OnTriggerExit2DAsObservable()
				.Subscribe(otherCollider2D => {
					if(TagUtil.IsTagPlayer(otherCollider2D.tag, true) &&
						!dialogueGetter.IsDialogueDone().Value) {
							dialogueSetter.StopDialogue();
					}
				})
				.AddTo(this);
		}

		dialogueGetter.IsDialogueDone().Where(done => done && isTriggered)
			.Subscribe(_ => OnFinishDialogue())
			.AddTo(this);

		this.OnTriggerEnter2DAsObservable()
			.Subscribe(otherCollider2D => {
				if(TagUtil.IsTagPlayer(otherCollider2D.tag, true) &&
					dialogueGetter.IsDialogueDone().Value) {
						OnStartDialogue();
				}
			})
			.AddTo(this);

	}

	private void OnStartDialogue() {
		timerSetter.PauseCountdown();

		if(toHalveBGMVolume) {
			volumeSetter.HalveVolumeBGM();
		}

		if(toDisablePlayerMovement) {
			playerInput.DisableControls();
		}

		dialogueSetter.ConfigFocusedObject(focusedObject);
		dialogueSetter.ConfigButtonText(textButtonNext, textButtonFinish);
		StartDialogue();
		isTriggered = true;
	}

	private void OnFinishDialogue() {
		timerSetter.StartCountdown();

		if(toHalveBGMVolume) {
			volumeSetter.UnHalveVolumeBGM();
		}

		playerInput.EnableControls();
		isTriggered = false;

		if(afterEvents != null && afterEvents.Length > 0) {
			foreach(InGameEvent afterEvent in afterEvents) {
				afterEvent.FireEvent();
			}
		}

		if(isOneTime) {
			Destroy(this.gameObject);
		}
	}


}

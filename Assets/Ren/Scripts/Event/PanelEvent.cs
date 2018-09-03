using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Zenject;

public class PanelEvent : InGameEvent {

	[SerializeField] private MaskableGraphic panel;
	[SerializeField] private Button[] buttonTriggers;
	[SerializeField] private bool shouldFreezeGamePlay;
	[Space]
	[SerializeField] private PlayableDirector playableOpen;
	[SerializeField] private PlayableDirector playableClose;
	[Space]
	[SerializeField] private InGameEvent afterEvent;

	[Inject] PlayerInputControls playerInput;
	[Inject] Timer_Setter timerSetter;

	private bool isTriggered;

	private void Awake() {
		if(panel == null) {
			LogUtil.PrintWarning(gameObject, GetType(), "No Panel specified. Destroying...");
			Destroy(this.gameObject);
		}
	}

	private void Start() {
		if(buttonTriggers != null && buttonTriggers.Length > 0) {
			foreach(Button button in buttonTriggers) {
				button.OnClickAsObservable()
					.Subscribe(isClicked => {
						if(isTriggered) {
							CancelNow();
						} else {
							FireEvent();
						}
					})
					.AddTo(this);
			}
		} else {
			LogUtil.PrintInfo(gameObject, GetType(), "There's no Button Triggers specified.");
		}

		panel.gameObject.SetActive(false);
	}

	protected override bool FireNow() {
		isTriggered = true;
		StartCoroutine(CorFirePlayableOpen());
		return true;
	}

	public override void CancelNow() {
		LogUtil.PrintInfo(gameObject, GetType(), "CancelNow() called...");
		StartCoroutine(CorFirePlayableClose());

		isTriggered = false;
	}

	private IEnumerator CorFirePlayableOpen() {
		panel.gameObject.SetActive(true);

		if(playableOpen == null) {
			yield return null;
		} else {
			playableOpen.gameObject.SetActive(true);
			playableOpen.Play();

			yield return new WaitForSeconds((float) playableOpen.duration);
		}

		if(shouldFreezeGamePlay) {
			timerSetter.PauseCountdown();
			playerInput.DisableControls();
		} 
	}

	private IEnumerator CorFirePlayableClose() {
		if(playableClose == null) {
			yield return null;
		} else {
			playableClose.gameObject.SetActive(true);
			playableClose.Play();

			yield return new WaitForSeconds((float) playableClose.duration);

			playableClose.gameObject.SetActive(false);
		}

		panel.gameObject.SetActive(false);

		if(shouldFreezeGamePlay) {
			timerSetter.StartCountdown();
			playerInput.EnableControls();
		} 

		if(afterEvent != null) {
			afterEvent.FireEvent();
		}
	}

}

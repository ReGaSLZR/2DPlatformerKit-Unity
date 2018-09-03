using UnityEngine;
using Zenject;

public class CutsceneEvent : InGameEvent
{

//	[SerializeField] private mp4player?

	[Inject] Timer_Setter timerSetter;

	private void Awake() {
//		autoCancelDelay = mp4player.duration?
	}

	protected override bool FireNow() {
		timerSetter.PauseCountdown();
		//TODO

		return true;
	}

	public override void CancelNow() {
		//TODO
		timerSetter.StartCountdown();
	}

}


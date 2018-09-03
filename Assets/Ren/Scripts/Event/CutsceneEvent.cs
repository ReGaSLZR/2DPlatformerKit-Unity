using UnityEngine;
using UnityEngine.Video;
using Zenject;

public class CutsceneEvent : InGameEvent
{

	[SerializeField] private VideoPlayer videoPlayer;
	[SerializeField] private float additionalEndDelay = 2f;

	[Inject] Timer_Setter timerSetter;
	[Inject] Canvas canvas;

	private void Awake() {
		autoCancelDelay = ((float) videoPlayer.clip.length + additionalEndDelay);
	}

	protected override bool FireNow() {
		videoPlayer.Play();
		canvas.gameObject.SetActive(false);

		timerSetter.PauseCountdown();

		return true;
	}

	public override void CancelNow() {
		canvas.gameObject.SetActive(true);
		videoPlayer.Stop();

		timerSetter.StartCountdown();
	}

}


using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;
using Zenject;

public class TestButtons : MonoBehaviour
{

	[SerializeField] private Button buttonTimer;
	[SerializeField] private Button buttonVolume;

	[Inject] Timer_Observer timerObserver;
	[Inject] Timer_Setter timerSetter;

	[Inject] VolumeController_Observer volumeObserver;
	[Inject] VolumeController_Setter volumeSetter;

	private void Start() {
		buttonTimer.OnPointerClickAsObservable()
			.Subscribe(_ => {
				if(timerObserver.IsCountdownOngoing()) {
					timerSetter.PauseCountdown();
				} else {
					timerSetter.StartCountdown();
				}
			})
			.AddTo(this);

		buttonVolume.OnPointerClickAsObservable()
			.Subscribe(_ => {
				volumeSetter.MuteVolume(!volumeObserver.IsMuted().Value);
			})
			.AddTo(this);

	}

}


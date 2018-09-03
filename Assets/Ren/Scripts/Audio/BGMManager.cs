using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour
{

	[SerializeField] private AudioClip[] gameplayBgms;
	[SerializeField] private bool shouldLoop;

	[Space]

	[SerializeField] private AudioClip gameOverBgm;

	[Inject] VolumeController_Observer volumeObserver;
	[Inject] PlayerStats_Observer playerStats;

	private AudioSource _audioSource;

	private void Awake() {
		_audioSource = GetComponent<AudioSource>();
		_audioSource.loop = shouldLoop;
	}

	private void Start() {
		_audioSource.volume = volumeObserver.GetVolumeBGM().Value;

		if(gameplayBgms.Length == 0) {
			LogUtil.PrintError(gameObject, GetType(), "No BGM clips defined!");
		}
		else {
			AudioUtil.PlayRandomClip(GetType(), gameplayBgms, _audioSource);
		}

		if(gameOverBgm != null) {
			playerStats.IsGameOver()
				.Where(isGameOver => isGameOver)
				.Subscribe(_ => {
					AudioUtil.PlaySingleClip(GetType(), gameOverBgm, _audioSource);
				})
				.AddTo(this);
		}
	}

}
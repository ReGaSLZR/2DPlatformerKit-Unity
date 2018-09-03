using UnityEngine;
using UniRx;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class VolumeAdjuster : MonoBehaviour {

	[Tooltip("NOTE: Runtime changes on 'audioType' won't take effect. This is to make the class listen to one Observable only.")]
	[SerializeField] private AUDIO_TYPE audioType;

	[Inject] VolumeController_Observer volumeObserver;

	private AudioSource _audioSource;

	private void Awake() {
		_audioSource = this.GetComponent<AudioSource>();
	}

	private void Start () {
		volumeObserver.IsMuted()
			.Subscribe(isMuted => _audioSource.mute = isMuted)
			.AddTo(this);

		if(AUDIO_TYPE.BGM == audioType) {
			volumeObserver.GetVolumeBGM()
				.Subscribe(bgmVol => _audioSource.volume = bgmVol)
				.AddTo(this);
		} else {
			volumeObserver.GetVolumeSFX()
				.Subscribe(sfxVol => _audioSource.volume = sfxVol)
				.AddTo(this);
		}
	}

}

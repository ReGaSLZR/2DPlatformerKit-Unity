using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Zenject;

public class VolumeController : MonoBehaviour,
								VolumeController_Observer,
								VolumeController_Setter
{

	[Header("UI Controllers")]
	[SerializeField] private Slider sliderBGM;
	[SerializeField] private Slider sliderSFX;
	[SerializeField] private ToggleButton muteController;

	private float maxVolume = 1f;
	private float originalVolumeBGM;

	private ReactiveProperty<float> volumeBGM;
	private ReactiveProperty<float> volumeSFX;
	private ReactiveProperty<bool> isMuted;

	private void Awake() {
		volumeBGM = new ReactiveProperty<float>();
		volumeSFX = new ReactiveProperty<float>();
		isMuted = new ReactiveProperty<bool>();
	}

	private void Start() {
		SetPropertyValues();
		SetUIDefaults();
		SetChangeListeners();
	}

	private void SetPropertyValues() {
		LogUtil.PrintInfo(gameObject, GetType(), "Init BGM value: " 
			+ PlayerPrefs.GetFloat(ConfigPrefs.KEY_FLOAT_AUDIO_VOLUME_BGM));
		volumeBGM.Value = (PlayerPrefs.GetFloat(ConfigPrefs.KEY_FLOAT_AUDIO_VOLUME_BGM));
		originalVolumeBGM = volumeBGM.Value;
		volumeSFX.Value = (PlayerPrefs.GetFloat(ConfigPrefs.KEY_FLOAT_AUDIO_VOLUME_SFX));
		isMuted.Value = (PlayerPrefs.GetInt(ConfigPrefs.KEY_INTBOOL_AUDIO_ISMUTED) == 1);
	}

	private void SetChangeListeners() {
		if(muteController == null) {
			LogUtil.PrintWarning(gameObject, GetType(), "muteController ToggleButton is NULL. :(");
		}
		else {
			muteController.isToggled
				.Subscribe(toggled => isMuted.Value = toggled)
				.AddTo(this);
		}

		if(sliderBGM == null) {
			LogUtil.PrintWarning(gameObject, GetType(), "sliderBGM is NULL. :(");
		}
		else {
			sliderBGM.OnValueChangedAsObservable()
				.Subscribe(value => volumeBGM.Value = value)
				.AddTo(this);
		}

		if(sliderSFX == null) {
			LogUtil.PrintWarning(gameObject, GetType(), "sliderSFX is NULL. :(");
		}
		else {
			sliderSFX.OnValueChangedAsObservable()
				.Subscribe(value => volumeSFX.Value = value)
				.AddTo(this);
		}
	}

	private void SetUIDefaults() {
		if((muteController != null) && (sliderBGM != null) && (sliderSFX != null)) {
			muteController.Toggle(isMuted.Value);
			sliderBGM.value = volumeBGM.Value;
			sliderSFX.value = volumeSFX.Value;
		}
	}

	public ReactiveProperty<float> GetVolumeBGM() {
		return volumeBGM;
	}

	public ReactiveProperty<float> GetVolumeSFX() {
		return volumeSFX;
	}

	public ReactiveProperty<bool> IsMuted() {
		return isMuted;
	}

	public void SetVolumeBGM(float volBGM) {
		volumeBGM.Value = (volBGM <= maxVolume) ? volBGM : maxVolume;
	}

	public void SetVolumeSFX(float volSFX) {
		volumeSFX.Value = (volSFX <= maxVolume) ? volSFX : maxVolume;
	}

	public void HalveVolumeBGM() {
		originalVolumeBGM = volumeBGM.Value;
		volumeBGM.Value = (originalVolumeBGM/2);
	}

	public void UnHalveVolumeBGM() {
		volumeBGM.Value = originalVolumeBGM;
	}

	public void MuteVolume(bool isMuted) {
		this.isMuted.Value = isMuted;
	}

}

/* INTERFACES */

public interface VolumeController_Observer {
	ReactiveProperty<float> GetVolumeBGM();
	ReactiveProperty<float> GetVolumeSFX();
	ReactiveProperty<bool> IsMuted();
}

public interface VolumeController_Setter {
	void SetVolumeBGM(float volBGM);
	void SetVolumeSFX(float volSFX);

	void HalveVolumeBGM();
	void UnHalveVolumeBGM();

	void MuteVolume(bool isMuted);
}
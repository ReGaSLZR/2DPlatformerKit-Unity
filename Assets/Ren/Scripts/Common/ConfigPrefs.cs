using UnityEngine;

public class ConfigPrefs : MonoBehaviour {

	private static string KEY_INTBOOL_IS_FIRST_RUN = "KEY_IS_FIRST_RUN";

	public static string KEY_INTBOOL_AUDIO_ISMUTED = "KEY_AUDIO_ISMUTED";
	public static string KEY_FLOAT_AUDIO_VOLUME_BGM = "KEY_AUDIO_VOLUME_BGM";
	public static string KEY_FLOAT_AUDIO_VOLUME_SFX = "KEY_AUDIO_VOLUME_SFX";

	[Range(0f, 1f)]
	[SerializeField] private float defaultVolumeBGM = 1f;
	[Range(0f, 1f)]
	[SerializeField] private float defaultVolumeSFX = 1f;
	[SerializeField] private bool isMutedStart = false;

	private void Awake() {
		if(PlayerPrefs.HasKey(KEY_INTBOOL_IS_FIRST_RUN)) {
			LogUtil.PrintWarning(gameObject, GetType(), "First time run detected. Setting config defaults.");
			PlayerPrefs.SetInt(KEY_INTBOOL_IS_FIRST_RUN, 1);

			PlayerPrefs.SetInt(KEY_INTBOOL_AUDIO_ISMUTED, (isMutedStart) ? 1 : 0);
			PlayerPrefs.SetFloat(KEY_FLOAT_AUDIO_VOLUME_BGM, defaultVolumeBGM);
			PlayerPrefs.SetFloat(KEY_FLOAT_AUDIO_VOLUME_SFX, defaultVolumeSFX);

//			PlayerPrefs.Save(); 
		} else {
			LogUtil.PrintInfo(gameObject, GetType(), "This isn't the 1st time the game is ran. Ignoring config defaults.");
		}
	}

}

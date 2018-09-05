using UnityEngine;

public class ConfigPrefs : MonoBehaviour {

	private static string KEY_INTBOOL_IS_FIRST_RUN = "KEY_IS_FIRST_RUN";

	public static string KEY_INTBOOL_AUDIO_ISMUTED = "KEY_AUDIO_ISMUTED";
	public static string KEY_FLOAT_AUDIO_VOLUME_BGM = "KEY_AUDIO_VOLUME_BGM";
	public static string KEY_FLOAT_AUDIO_VOLUME_SFX = "KEY_AUDIO_VOLUME_SFX";

	public static string KEY_INT_HEALTH = "KEY_HEALTH";
	public static string KEY_INT_LIVES = "KEY_LIVES";
	public static string KEY_INT_SHOTS = "KEY_SHOTS";
	public static string KEY_INT_MONEY = "KEY_MONEY";
	public static string KEY_INT_SCROLLS = "KEY_SCROLLS";

	public static string KEY_STRING_HIGHEST_LEVEL_CLEARED = "KEY_HIGHEST_LEVEL_CLEARED";

	[Header("Audio config")]
	[Range(0f, 1f)]
	[SerializeField] private float defaultVolumeBGM = 1f;
	[Range(0f, 1f)]
	[SerializeField] private float defaultVolumeSFX = 1f;
	[SerializeField] private bool isMutedStart = false;

	[Header("Player Starting Stats config")]
	[SerializeField]
	[SerializeField] private int health = 3;
	[SerializeField] private int lives = 5;
	[SerializeField] private int shots = 5;
	[SerializeField] private int money = 0;
	[SerializeField] private int scrolls = 0;

	private void Awake() {
		if(!PlayerPrefs.HasKey(KEY_INTBOOL_IS_FIRST_RUN)) {
			LogUtil.PrintWarning(gameObject, GetType(), "First time run detected. Setting config defaults.");
			PlayerPrefs.SetInt(KEY_INTBOOL_IS_FIRST_RUN, 1);

			//AUDIO
			PlayerPrefs.SetInt(KEY_INTBOOL_AUDIO_ISMUTED, (isMutedStart) ? 1 : 0);
			PlayerPrefs.SetFloat(KEY_FLOAT_AUDIO_VOLUME_BGM, defaultVolumeBGM);
			PlayerPrefs.SetFloat(KEY_FLOAT_AUDIO_VOLUME_SFX, defaultVolumeSFX);

			//PLAYER STATS
			PlayerPrefs.SetInt(KEY_INT_HEALTH, health);
			PlayerPrefs.SetInt(KEY_INT_LIVES, lives);
			PlayerPrefs.SetInt(KEY_INT_SHOTS, shots);
			PlayerPrefs.SetInt(KEY_INT_MONEY, money);
			PlayerPrefs.SetInt(KEY_INT_SCROLLS, scrolls);

			PlayerPrefs.SetString(KEY_STRING_HIGHEST_LEVEL_CLEARED, StringCipher.Encrypt(SceneUtil.NON_WORLD_LEVEL_SCENES)); 

			PlayerPrefs.Save(); 
		} else {
			LogUtil.PrintInfo(gameObject, GetType(), "This isn't the 1st time the game is ran. Ignoring config defaults.");
		}
	}

}

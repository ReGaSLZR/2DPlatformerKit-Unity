using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPrefsUtil {

	private static string KEY_INTBOOL_IS_FIRST_RUN = "KEY_IS_FIRST_RUN";
	private static string KEY_STRING_SCROLL_LIST = "KEY_SCROLL_LIST";

	private static int ON_RETRY_BONUS_SHOTS_COUNT = 300;

	public static string KEY_INTBOOL_AUDIO_ISMUTED = "KEY_AUDIO_ISMUTED";
	public static string KEY_FLOAT_AUDIO_VOLUME_BGM = "KEY_AUDIO_VOLUME_BGM";
	public static string KEY_FLOAT_AUDIO_VOLUME_SFX = "KEY_AUDIO_VOLUME_SFX";

	public static string KEY_INT_HEALTH = "KEY_HEALTH";
	public static string KEY_INT_LIVES = "KEY_LIVES";
	public static string KEY_INT_SHOTS = "KEY_SHOTS";
	public static string KEY_INT_MONEY = "KEY_MONEY";
	public static string KEY_INT_SCROLLS = "KEY_SCROLLS";

	//ENCRYPTED
	public static string KEY_STRING_HIGHEST_LEVEL_CLEARED = "KEY_HIGHEST_LEVEL_CLEARED";

	public static void ConfigFirstRun(
		bool isMutedStart,
		float defaultVolumeBGM,
		float defaultVolumeSFX,
		int health,
		int lives,
		int shots,
		int money,
		int scrolls)
	{
			if(!PlayerPrefs.HasKey(KEY_INTBOOL_IS_FIRST_RUN)) {

				LogUtil.PrintWarning("PlayerPrefsUtil: First time run detected. Setting config defaults.");
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
				LogUtil.PrintInfo("PlayerPrefsUtil: This isn't the 1st time the game is ran. Ignoring config defaults.");
			}
	}

	public static void ConfigRetryStats() {
		LogUtil.PrintWarning("PlayerPrefsUtil: ConfigRetryStats()");

		//PLAYER STATS
		PlayerPrefs.SetInt(KEY_INT_HEALTH, 3);
		PlayerPrefs.SetInt(KEY_INT_LIVES, 5);

		if(PlayerPrefs.GetInt(KEY_INT_SHOTS, 0) < ON_RETRY_BONUS_SHOTS_COUNT) {
			PlayerPrefs.SetInt(KEY_INT_SHOTS, ON_RETRY_BONUS_SHOTS_COUNT);
		}

		PlayerPrefs.Save(); 
	}

	public static void SaveStats(VolumeController_Observer volumeStats, PlayerStats_Observer playerStats) {
		PlayerPrefs.SetString(KEY_STRING_HIGHEST_LEVEL_CLEARED, 
			StringCipher.Encrypt(SceneManager.GetActiveScene().buildIndex.ToString()));

		PlayerPrefs.SetInt(KEY_INTBOOL_AUDIO_ISMUTED, (volumeStats.IsMuted().Value) ? 1 : 0);
		PlayerPrefs.SetFloat(KEY_FLOAT_AUDIO_VOLUME_BGM, volumeStats.GetVolumeBGM().Value);
		PlayerPrefs.SetFloat(KEY_FLOAT_AUDIO_VOLUME_SFX, volumeStats.GetVolumeSFX().Value);

		PlayerPrefs.SetInt(KEY_INT_HEALTH, playerStats.GetHealth().Value);
		PlayerPrefs.SetInt(KEY_INT_LIVES, playerStats.GetLives().Value);
		PlayerPrefs.SetInt(KEY_INT_SHOTS, playerStats.GetShots().Value);
		PlayerPrefs.SetInt(KEY_INT_MONEY, playerStats.GetShots().Value);
//		PlayerPrefs.SetInt(KEY_INT_SCROLLS, playerStats.GetScrolls().Value);

		PlayerPrefs.Save();
	}

	public static int GetLatestLevel() {
		string latestLevel = PlayerPrefs.GetString(KEY_STRING_HIGHEST_LEVEL_CLEARED, null);
		latestLevel = (StringUtil.IsNonNullNonEmpty(latestLevel)) ? StringCipher.Decrypt(latestLevel) 
			: SceneUtil.NON_WORLD_LEVEL_SCENES;
		return int.Parse(latestLevel);
	}

	public static void SaveScroll(string scrollKey) {
		if(!IsScrollKeyTaken(scrollKey)) {
			LogUtil.PrintInfo("PlayerPrefsUtil: Saving Scroll count");

			int tempScrollCount = PlayerPrefs.GetInt(KEY_INT_SCROLLS, 0);
			tempScrollCount++;
			PlayerPrefs.SetInt(KEY_INT_SCROLLS, tempScrollCount);

			string tempScrollKeyList = PlayerPrefs.GetString(KEY_STRING_SCROLL_LIST, "");
			tempScrollKeyList += ("-" + scrollKey + "-");
			PlayerPrefs.SetString(KEY_STRING_SCROLL_LIST, tempScrollKeyList);

			PlayerPrefs.Save();
		}
	}

	public static bool IsScrollKeyTaken(string scrollKey) {
		string tempScrollKeyList = PlayerPrefs.GetString(KEY_STRING_SCROLL_LIST, "");
		return tempScrollKeyList.Contains(scrollKey);
	}

}

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUtil {

	private static int SPLASH = 0;
	private static int MAIN_MENU = 1;
	private static int LOADING = 2;

	//NOTE: 2 == default highest level cleared in-game | Zero-based | refer to SceneIndexes
	public static string NON_WORLD_LEVEL_SCENES = "2";

	public static int GetLatestLevel() {
		string latestLevel = PlayerPrefs.GetString(ConfigPrefs.KEY_STRING_HIGHEST_LEVEL_CLEARED, null);
		latestLevel = (StringUtil.IsNonNullNonEmpty(latestLevel)) ? StringCipher.Decrypt(latestLevel) : NON_WORLD_LEVEL_SCENES;
		return int.Parse(latestLevel);
	}

	public static void SaveCurrentSceneStats(VolumeController_Observer volumeStats, PlayerStats_Observer playerStats) {
		PlayerPrefs.SetString(ConfigPrefs.KEY_STRING_HIGHEST_LEVEL_CLEARED, 
			StringCipher.Encrypt(SceneManager.GetActiveScene().buildIndex.ToString()));

		PlayerPrefs.SetInt(ConfigPrefs.KEY_INTBOOL_AUDIO_ISMUTED, (volumeStats.IsMuted().Value) ? 1 : 0);
		PlayerPrefs.SetFloat(ConfigPrefs.KEY_FLOAT_AUDIO_VOLUME_BGM, volumeStats.GetVolumeBGM().Value);
		PlayerPrefs.SetFloat(ConfigPrefs.KEY_FLOAT_AUDIO_VOLUME_SFX, volumeStats.GetVolumeSFX().Value);

		PlayerPrefs.SetInt(ConfigPrefs.KEY_INT_HEALTH, playerStats.GetHealth().Value);
		PlayerPrefs.SetInt(ConfigPrefs.KEY_INT_LIVES, playerStats.GetLives().Value);
		PlayerPrefs.SetInt(ConfigPrefs.KEY_INT_SHOTS, playerStats.GetShots().Value);
		PlayerPrefs.SetInt(ConfigPrefs.KEY_INT_MONEY, playerStats.GetShots().Value);
		PlayerPrefs.SetInt(ConfigPrefs.KEY_INT_SCROLLS, playerStats.GetScrolls().Value);

		PlayerPrefs.Save();
	}

	public static void LoadMainMenu() {
		SceneManager.LoadSceneAsync(LOADING, LoadSceneMode.Additive);
		SceneManager.LoadSceneAsync(MAIN_MENU);
	}

	public static void LoadScene(int index) {
		SceneManager.LoadSceneAsync(LOADING, LoadSceneMode.Additive);
		SceneManager.LoadSceneAsync(index);
	}

}

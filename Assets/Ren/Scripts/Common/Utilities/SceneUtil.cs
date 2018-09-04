using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUtil {

	private static int SPLASH = 0;
	private static int MAIN_MENU = 1;
	private static int LOADING = 2;

	private static int NON_WORLD_LEVEL_SCENES = 2; //ZERO-BASED

	public static int GetLatestLevel() {
		return PlayerPrefs.GetInt(ConfigPrefs.KEY_INT_HIGHEST_LEVEL_CLEARED, NON_WORLD_LEVEL_SCENES);
	}

	public static void SaveCurrentLevel_AsCleared() {
		PlayerPrefs.SetInt(ConfigPrefs.KEY_INT_HIGHEST_LEVEL_CLEARED, SceneManager.GetActiveScene().buildIndex);
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

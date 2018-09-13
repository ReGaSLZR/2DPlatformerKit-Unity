using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUtil {

//	private static int SPLASH = 0;
	private static int MAIN_MENU = 1;
	private static int LOADING = 2;

	//NOTE: 2 == default highest level cleared in-game | Zero-based | refer to SceneIndexes
	public static string NON_WORLD_LEVEL_SCENES = "2";

	public static void LoadMainMenu() {
		SceneManager.LoadSceneAsync(LOADING, LoadSceneMode.Additive);
		SceneManager.LoadSceneAsync(MAIN_MENU);
	}

	public static void LoadScene(int index) {
		SceneManager.LoadSceneAsync(LOADING, LoadSceneMode.Additive);
		SceneManager.LoadSceneAsync(index);
	}

	public static int GetSceneIndex_Current() {
		return SceneManager.GetActiveScene().buildIndex;
	}

	public static string GetSceneName_Current() {
		return SceneManager.GetActiveScene().name;
	}

}

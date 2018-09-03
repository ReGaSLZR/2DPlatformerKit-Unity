using UnityEngine.SceneManagement;

public class SceneIndexes {

	private static int SPLASH = 0;
	private static int MAIN_MENU = 1;
	private static int LOADING = 2;

	public static void LoadMainMenu() {
		SceneManager.LoadSceneAsync(LOADING, LoadSceneMode.Additive);
		SceneManager.LoadSceneAsync(MAIN_MENU);
	}

	public static void LoadScene(int index) {
		SceneManager.LoadSceneAsync(LOADING, LoadSceneMode.Additive);
		SceneManager.LoadSceneAsync(index);
	}

}

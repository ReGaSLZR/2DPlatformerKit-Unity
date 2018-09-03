using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Playables;
using UniRx;
using UniRx.Triggers;
using Zenject;

public class SplashStateController : MonoBehaviour {

	[SerializeField] private Button playButton;
	[SerializeField] private PlayableDirector playableOnButtonTick;
	[SerializeField] private AudioSource bgmHolder;

	[Inject] SceneIndexes sceneIndexes;

	private void Start() {
		playButton.OnClickAsObservable()
			.Subscribe(_ => {
				StartCoroutine(CorLoadMainMenu());
			})
			.AddTo(this);
	}

	private IEnumerator CorLoadMainMenu() {
		LogUtil.PrintInfo(gameObject, GetType(), "Splash Over. Loading Main Menu.");
		bgmHolder.Stop();

		if(playableOnButtonTick != null) {
			playableOnButtonTick.gameObject.SetActive(true);
			playableOnButtonTick.Play();

			yield return new WaitForSeconds((float) playableOnButtonTick.duration);
		}

		SceneManager.LoadScene(sceneIndexes.LOADING, LoadSceneMode.Additive);
		SceneManager.LoadSceneAsync(sceneIndexes.MAIN_MENU);
	}
}

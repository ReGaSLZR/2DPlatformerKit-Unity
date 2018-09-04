using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UniRx;
using UniRx.Triggers;

public class SplashStateController : MonoBehaviour {

	[SerializeField] private Button playButton;
	[SerializeField] private PlayableDirector playableOnButtonTick;
	[SerializeField] private AudioSource bgmHolder;

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
			
		SceneUtil.LoadMainMenu();
	}
}

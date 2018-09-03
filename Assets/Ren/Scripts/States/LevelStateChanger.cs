using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Zenject;

[RequireComponent(typeof(Collider2D))]
public class LevelStateChanger : MonoBehaviour {

	[SerializeField] private LEVEL_CHANGER_TYPE type;
	[SerializeField] private PlayableDirector playable;

	[Inject] InputControlDisabler inputDisabler;
	[Inject] SceneIndexes sceneIndexes;

	private IEnumerator CorPlayLevelState() {
		LogUtil.PrintInfo(gameObject, GetType(), "LEVEL STATE change!");
		inputDisabler.DisableControls();

		if(playable != null) {
			playable.gameObject.SetActive(true);
			playable.Play();
			yield return new WaitForSeconds((float) playable.duration);
		}

		if(LEVEL_CHANGER_TYPE.STARTER == type) {
			inputDisabler.EnableControls();
		} else {
			SceneManager.LoadScene(sceneIndexes.LOADING, LoadSceneMode.Additive);
			SceneManager.LoadSceneAsync(sceneIndexes.MAIN_MENU);
		}

		Destroy(this);
	}

	private void OnTriggerEnter2D(Collider2D otherCollider2D) {
		if(TagUtil.IsTagPlayer(otherCollider2D.tag, true)) {
			StopAllCoroutines();
			StartCoroutine(CorPlayLevelState());
		}
	}

}

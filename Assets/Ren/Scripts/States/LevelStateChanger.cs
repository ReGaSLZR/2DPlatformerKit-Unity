using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using Zenject;

[RequireComponent(typeof(Collider2D))]
public class LevelStateChanger : MonoBehaviour {

	[SerializeField] private LEVEL_CHANGER_TYPE type;
	[SerializeField] private PlayableDirector playable;

	[Inject] InputControlDisabler inputDisabler;

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
			SceneUtil.SaveCurrentLevel_AsCleared();
			SceneUtil.LoadMainMenu();
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

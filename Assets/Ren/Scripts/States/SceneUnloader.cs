using UnityEngine;
using System.Collections;

public class SceneUnloader : MonoBehaviour
{

	[SerializeField] private int sceneIndexToUnload = SceneUtil.LOADING;
	[SerializeField] private float unloadDelay = 7f;

	private void Start()
	{
		StartCoroutine(CorUnloadScene());
	}

	private IEnumerator CorUnloadScene() {
		yield return new WaitForSeconds(unloadDelay);

		LogUtil.PrintInfo(gameObject, GetType(), "Unloading Scene with index " + sceneIndexToUnload);
		SceneUtil.UnloadScene(sceneIndexToUnload);
	}

	private void OnDisable() {
		StopAllCoroutines();
	}

}


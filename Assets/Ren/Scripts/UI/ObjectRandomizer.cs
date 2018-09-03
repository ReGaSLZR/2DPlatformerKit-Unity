using System.Collections;
using UnityEngine;

public class ObjectRandomizer : MonoBehaviour {

	[SerializeField] private GameObject[] objects;
	[SerializeField] private float randomizationDelay = 0f;
	[SerializeField] private float delayStart = 0f;

	private int currentIndex = -1;

	private void Awake() {
		if(objects == null || objects.Length <= 1) {
			LogUtil.PrintWarning(gameObject, GetType(), "No GameObject Array defined... Destroying.");
			Destroy(this.gameObject);
		}

		DeactivateAllObjects();
	}

	private void Start() {
		StartCoroutine(CorRandomizeObject());
	}

	private void DeactivateAllObjects() {
		foreach(GameObject gameObject in objects) {
			gameObject.SetActive(false);
		}
	}

	private IEnumerator CorRandomizeObject() {
		yield return new WaitForSeconds(delayStart);

		do{
			ResetCurrentIndex();
			objects[currentIndex].SetActive(true);

			yield return new WaitForSeconds(randomizationDelay);
			objects[currentIndex].SetActive(false);
		} while(randomizationDelay > 0f);
	}

	private void ResetCurrentIndex() {
		int newIndex;

		do {
			newIndex = Random.Range(0, objects.Length);
		} while(newIndex == currentIndex);

		currentIndex = newIndex;
	}

}

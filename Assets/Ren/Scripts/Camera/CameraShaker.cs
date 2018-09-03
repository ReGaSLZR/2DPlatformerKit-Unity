using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour,
							CameraShake_Setter
{

	[SerializeField] private float duration = 1f;
	[SerializeField] private float magnitude = 1f;

	[SerializeField] private float axisShakeMin = -1f;
	[SerializeField] private float axisShakeMax = 1f;

	private IEnumerator Shake() {
		float timeElapsed = 0f;
		Vector3 originalPosition = transform.localPosition;

		while(timeElapsed < duration) {
			float x = Random.Range(axisShakeMin, axisShakeMax) * magnitude;
			float y = Random.Range(axisShakeMin, axisShakeMax) * magnitude;

			transform.position = new Vector3(x, y, originalPosition.z);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		transform.localPosition = originalPosition;

	}

	public void StartShaking() {
		StopAllCoroutines();
		StartCoroutine(Shake());
	}

	public void StopShaking() {
		StopAllCoroutines();	
	}

}

public interface CameraShake_Setter {

	void StartShaking();
	void StopShaking();

}


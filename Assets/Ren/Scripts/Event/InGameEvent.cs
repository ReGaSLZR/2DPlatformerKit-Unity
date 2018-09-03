using System.Collections;
using UnityEngine;

public abstract class InGameEvent : MonoBehaviour
{

	[SerializeField] public bool autoCancel;
	[SerializeField] protected float autoCancelDelay;
	[Space]
	[SerializeField] private bool cancelOnStart;
	[SerializeField] private bool destroyOnEnd;

	private void Awake() {
		if(cancelOnStart) {
			CancelNow();
		}
	}

	public bool FireEvent() {
		bool isSuccessful = FireNow();

		if(autoCancel) {
			StartCoroutine(CorDelayCancel());
		}

		return isSuccessful;
	}

	private IEnumerator CorDelayCancel() {
		yield return new WaitForSeconds(autoCancelDelay);
		CancelNow();

		if(destroyOnEnd) {
			Destroy(this.gameObject);
		}
	}

	protected abstract bool FireNow();
	public abstract void CancelNow();

}
using System.Collections;
using UnityEngine;
using Zenject;

public class LifetimeTimer : MonoBehaviour
{

	[SerializeField] private float lifetime = 2f;
	[SerializeField] private GameObject prefabSub;

	[Inject] Instantiator instantiator;

	private void Awake() {
		StartCoroutine(CorStartLifetime());
	}

	private IEnumerator CorStartLifetime() {
		yield return new WaitForSeconds(lifetime);

		if(prefabSub != null) {
			GameObject sub = Instantiate(prefabSub,
				this.gameObject.transform.position,
				this.gameObject.transform.rotation);

			instantiator.InjectPrefab(sub);
		}

		Destroy(this.gameObject);
	}

}


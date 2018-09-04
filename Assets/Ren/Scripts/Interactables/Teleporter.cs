using System.Collections;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

[RequireComponent(typeof(Collider2D))]
public class Teleporter : MonoBehaviour
{

	[SerializeField] private Transform destination;
	[SerializeField] private GameObject prefabTeleportFX;

	[Range(0, 2)]
	[SerializeField] private float delayTeleport;

	[Header("Is Single Teleportation?")]
	[SerializeField] private GameObject prefabDestroyFX;
	[SerializeField] private bool isOneTime;
	[Range(0, 2)]
	[SerializeField] private float delayDestroy;

	[Inject] Instantiator instantiator;

	private void Awake()
	{
		if(destination == null)
		{
			LogUtil.PrintError(gameObject, GetType(), "Destination transform is NULL.");
			Destroy(this.gameObject);
		}
	}

	private void Start()
	{
		this.OnTriggerEnter2DAsObservable()
			.Where(otherCollider2D => !(otherCollider2D.tag.Equals(OBJECT_TAG.Untagged.ToString())))
			.Subscribe(otherCollider2D => {
				StopAllCoroutines();
				StartCoroutine(CorTeleport(otherCollider2D));
			})
			.AddTo(this);
	}

	private IEnumerator CorTeleport(Collider2D otherCollider2D)
	{
		instantiator.InjectPrefab(prefabTeleportFX, otherCollider2D.gameObject);
		yield return new WaitForSeconds(delayTeleport);

		otherCollider2D.gameObject.transform.position = destination.position;
		instantiator.InjectPrefab(prefabTeleportFX, otherCollider2D.gameObject);

		if(isOneTime)
		{
			instantiator.InjectPrefab(prefabDestroyFX, this.gameObject);
			instantiator.InjectPrefab(prefabDestroyFX, destination.gameObject);

			yield return new WaitForSeconds(delayDestroy);

			Destroy(destination.gameObject);
			Destroy(this.gameObject);
		}
	}

}


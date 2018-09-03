using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Checkpoint : MonoBehaviour {

	[SerializeField] private Transform spawnPoint;
	[SerializeField] private GameObject prefabTriggerFX;
	[SerializeField] private float alphaOnTrigger = 0.5f;

	[Inject] Checkpoint_Setter checkSetter;
	[Inject] Instantiator instantiator;

	private SpriteRenderer _spriteRenderer;

	private void Awake() {
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void OnTriggerEnter2D(Collider2D otherCollider2D) {
		if(TagUtil.IsTagPlayer(otherCollider2D.tag, false)) {
			checkSetter.SetCheckpoint(spawnPoint.position, this.gameObject.name);

			InstantiateFX();
			DimAlpha();

			Destroy(this);
		}
	}

	private void InstantiateFX() {
		GameObject fx = Instantiate(prefabTriggerFX, gameObject.transform.position, gameObject.transform.rotation);
		instantiator.InjectPrefab(fx);
	}

	private void DimAlpha() {
		Color tempColor = _spriteRenderer.color;
		tempColor.a = alphaOnTrigger;
		_spriteRenderer.color = tempColor;
	}

}

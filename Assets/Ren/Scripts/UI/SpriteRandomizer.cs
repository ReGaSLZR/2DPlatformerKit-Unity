using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRandomizer : MonoBehaviour {

	[SerializeField] private Sprite[] sprites;

	private SpriteRenderer _spriteRenderer;

	private void Awake () {
		_spriteRenderer = GetComponent<SpriteRenderer>();

		if((sprites == null) || (sprites.Length == 0)) {
			LogUtil.PrintWarning(gameObject, GetType(), "No sprites defined!");
			Destroy(this);
		}
	}

	private void Start() {
		_spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
	}
}

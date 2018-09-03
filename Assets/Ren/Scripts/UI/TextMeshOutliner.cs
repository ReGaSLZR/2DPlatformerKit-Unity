using UnityEngine;

/*
	NOTE: Takes two TextMesh gameObjects with the other one serving as the shadown/outline.
		  What this script does is that it reflects the text supplied to it on runtime to the two TextMeshes
*/
public class TextMeshOutliner : MonoBehaviour {

	[SerializeField] private TextMesh textBase;
	[SerializeField] private TextMesh textOutline;

	private void Awake() {
		if((textBase == null) || (textOutline == null)) {
			LogUtil.PrintWarning(gameObject, GetType(), "TextMesh pair is missing.");
		} else {
			MeshRenderer baseRenderer = textBase.gameObject.GetComponent<MeshRenderer>();
			baseRenderer.sortingLayerName = "Foreground";
			baseRenderer.sortingOrder = 50;

			MeshRenderer outlineRenderer = textOutline.gameObject.GetComponent<MeshRenderer>();
			outlineRenderer.sortingLayerName = "Foreground";
			outlineRenderer.sortingOrder = 45;
		}
	}

	public void SetText(string text) {
		textBase.text = text;
		textOutline.text = text;
	}

}

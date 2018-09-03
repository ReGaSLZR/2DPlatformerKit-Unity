using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextChangerOverTime : MonoBehaviour {

	[SerializeField] private string[] textVariation;
	[SerializeField] private float textChangeDelay = 1f;

	private TextMeshProUGUI textMeshPro;

	private void Awake() {
		textMeshPro = GetComponent<TextMeshProUGUI>();
	}

	private void OnEnable() {
		if(textVariation != null && textVariation.Length > 0) {
			StartCoroutine(CorStartTextChange());
		}
		else {
			LogUtil.PrintError(gameObject, GetType(), "No textVariation array defined!");
		}
	}

	private IEnumerator CorStartTextChange() {
		for(int x=0; x<textVariation.Length; x++) {

			textMeshPro.text = textVariation[x];
			yield return new WaitForSeconds(textChangeDelay);

			if(x == (textVariation.Length-1)) {
				x=-1;
			}
		}
	}

	private void OnDisable() {
		StopAllCoroutines();	
	}

}

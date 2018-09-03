using System.Collections;
using UnityEngine;
using UniRx;
using TMPro;

public class DialogueLineFeeder : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI textToFeed;
	[SerializeField] private float letterRevealDuration = 0.025f;
	[SerializeField] private float lineBreakOnFullReveal = 5f;

	public ReactiveProperty<bool> isDone { get; private set; }

	private void Awake() {
		if(textToFeed == null) {
			LogUtil.PrintWarning(gameObject, GetType(), "Unfortunately, TextToFeed is unset.");
			Destroy(this);
		}

		isDone = new ReactiveProperty<bool>(false);
		textToFeed.SetText("");
	}

	public void FeedLine(string line) {
		StopAllCoroutines();
		textToFeed.SetText("");
		StartCoroutine(CorFeedLine(line));
	}

	public void StopOngoingFeed() {
		StopAllCoroutines();
	}

	private IEnumerator CorFeedLine(string line) {
		isDone.Value = false;
		char[] charArray = line.ToCharArray();
		string displayText = "";

		for(int x=0; x<charArray.Length; x++) {
			displayText += charArray[x];
			textToFeed.SetText(displayText);
			yield return new WaitForSeconds(letterRevealDuration);
		}

		yield return new WaitForSeconds(lineBreakOnFullReveal);

		isDone.Value = true;
	}

}

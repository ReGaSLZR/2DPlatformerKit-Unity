using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(Button))]
public class EventButton : MonoBehaviour {

	[SerializeField] private bool isOneTime;
	[Space]
	[SerializeField] private InGameEvent[] inGameEvents;

	private Button button;

	private void Awake() {
		button = GetComponent<Button>();
	}
	
	private void Start() {
		button.OnClickAsObservable()
			.Subscribe(_ => FireEvents())
			.AddTo(this);
	}

	private void FireEvents() {
		if(isOneTime) {
			button.interactable = false;
		}

		if((inGameEvents != null) && (inGameEvents.Length > 0)) {
			foreach(InGameEvent inGameEvent in inGameEvents) {
				inGameEvent.FireEvent();
			}
		}
	}

}

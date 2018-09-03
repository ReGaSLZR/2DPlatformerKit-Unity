using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using TMPro;

[RequireComponent(typeof(Button))]
public class ToggleButton : MonoBehaviour {

	private const string GET_DEFAULT_TEXT_ON_LABEL = "DEF";

	[SerializeField] private TextMeshProUGUI labelText;
	[SerializeField] private string labelOn;
	[SerializeField] private string labelOff = GET_DEFAULT_TEXT_ON_LABEL;
	[Space]
	[SerializeField] private bool defaultStateOn;
	[SerializeField] private bool isOneTime;
	[Space]
	[SerializeField] private InGameEvent[] inGameEvents;
	[SerializeField] private string onFailedEventSpiel = "???";
	[SerializeField] private Color onFailedEventColor = Color.red;

	public ReactiveProperty<bool> isToggled;

	private Button button;

	private void Awake() {
		if(labelOff.Equals(GET_DEFAULT_TEXT_ON_LABEL)) {
			labelOff = labelText.text;
		}

		isToggled = new ReactiveProperty<bool>(defaultStateOn);
		labelText.text = (defaultStateOn) ? labelOn : labelOff;

		button = GetComponent<Button>();
	}

	private void Start() {
		button.OnClickAsObservable()
			.Subscribe(_ => Toggle(!isToggled.Value))
			.AddTo(this);
	}

	public void Toggle(bool toggled) {
		isToggled.Value = toggled;
		labelText.text = (isToggled.Value) ? labelOn : labelOff;
		FireEvents();

		if(isOneTime) {
			button.interactable = false;
		}
	}

	private void FireEvents() {
		if(isToggled.Value && (inGameEvents != null) && (inGameEvents.Length > 0)) {
			foreach(InGameEvent inGameEvent in inGameEvents) {
				bool isSuccess = inGameEvent.FireEvent();

				if(!isSuccess) {
					labelText.text = onFailedEventSpiel;
					labelText.color = onFailedEventColor;

					return;
				}
			}
		}
	}
	

}

using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using TMPro;

[RequireComponent(typeof(Button))]
public class ToggleButton : MonoBehaviour {

	private const string GET_DEFAULT_TEXT_ON_LABEL = "DEF";

	[Tooltip("Label Text is required.")]
	[SerializeField] private TextMeshProUGUI labelText;
	[SerializeField] private string labelOn = GET_DEFAULT_TEXT_ON_LABEL;
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
		if(labelText == null) {
			LogUtil.PrintError(gameObject, GetType(), "No labelText defined. Destroying...");
			Destroy(this);
		}

		CheckDefaultLabels();

		isToggled = new ReactiveProperty<bool>(defaultStateOn);
		labelText.text = (defaultStateOn) ? labelOn : labelOff;

		button = GetComponent<Button>();
	}

	private void Start() {
		button.OnClickAsObservable()
			.Subscribe(_ => Toggle(!isToggled.Value))
			.AddTo(this);
	}

	private void CheckDefaultLabels() {
		labelOff = SetDefaultLabel(labelOff);
		labelOn = SetDefaultLabel(labelOn);
	}

	private string SetDefaultLabel(string labelTempValue) {
		return (labelTempValue.Equals(GET_DEFAULT_TEXT_ON_LABEL)) ? labelText.text : labelTempValue;
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

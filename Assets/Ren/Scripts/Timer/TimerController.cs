using UnityEngine;
using UniRx;
using UniRx.Triggers;
using TMPro;
using Zenject;

public class TimerController : MonoBehaviour, 
								Timer_Observer,
								Timer_Setter
{

	[SerializeField] private int countdownMax;
	[SerializeField] private int countdownStarting;
	[SerializeField] private bool shouldAutoStartCountdown;

	[Header("UI Specifics")]
	[SerializeField] private TextMeshProUGUI timerText;
	[SerializeField] private Color textColorSafe;
	[SerializeField] private Color textColorDanger;
	[SerializeField] private int dangerColorStart;

	private const int DEFAULT_INTERVAL_TIMER_TICK = 1;
	private bool isOngoing;

	private ReactiveProperty<int> countdown;
	private ReactiveProperty<bool> isCountdownOver;

	private void Awake() {
		countdown = new ReactiveProperty<int>(countdownStarting);
		PrepareTimerText();

		Observable.Interval(System.TimeSpan.FromSeconds(DEFAULT_INTERVAL_TIMER_TICK))
			.Where(_ => (isOngoing) && (countdown.Value > 0))
			.Subscribe(_ => { 
				--(countdown.Value);
				UpdateTimerText();
			})
			.AddTo(this);

		isCountdownOver = countdown.Select(timer => { 
			bool isOver = (timer <= 0);

			if(isOver) { isOngoing = false; }

			return isOver; 
		}).ToReactiveProperty();

	}

	private void Start() {
		if(shouldAutoStartCountdown) {
			isOngoing = true;
		}
	}

	private void PrepareTimerText() {
		if(timerText != null) {
			timerText.color = textColorSafe;
			timerText.text = countdownStarting.ToString();
		}
	}

	private void UpdateTimerText() {
		if(timerText != null) {
			timerText.text = countdown.Value.ToString();
			timerText.color = (countdown.Value <= dangerColorStart) ? textColorDanger : textColorSafe;
		}
	}
		
	public ReactiveProperty<int> GetCountdown() {
		return countdown;
	}

	public ReactiveProperty<bool> IsCountdownOver() {
		return isCountdownOver;
	}

	public bool IsCountdownOngoing() {
		return isOngoing;
	}

	public void PauseCountdown() {
		LogUtil.PrintInfo(gameObject, GetType(), "Pausing countdown...");
		isOngoing = false;
	}

	public void StartCountdown() {
		LogUtil.PrintInfo(gameObject, GetType(), "Starting/Resuming countdown...");
		isOngoing = true;
	}

	public void StopCountdown() {
		countdown.Value = 0;
	}

	public bool AddToCountdown(int seconds) {
		if(seconds > 0) {
			int tempNewCountdown = (countdown.Value + seconds);
			countdown.Value = (tempNewCountdown <= countdownMax) ? tempNewCountdown : countdownMax;
			return true;
		} 

		LogUtil.PrintWarning(gameObject, GetType(), "Cannot AddToCountdown() with value " + seconds);
		return false;
	}

	public bool DeductToCountdown(int seconds) {
		int tempNewCountdown = (countdown.Value - seconds);
		countdown.Value = (tempNewCountdown > 0) ? tempNewCountdown : 0;

		return (tempNewCountdown > 0);
	}

}

public interface Timer_Observer {

	ReactiveProperty<int> GetCountdown();
	ReactiveProperty<bool> IsCountdownOver();
	bool IsCountdownOngoing();

}

public interface Timer_Setter {

	void PauseCountdown();
	void StartCountdown();
	void StopCountdown();

	bool AddToCountdown(int seconds);
	bool DeductToCountdown(int seconds);

}

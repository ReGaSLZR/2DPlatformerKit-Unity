using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;
using Zenject;

public class GameStateChanger : MonoBehaviour {

	[Header("GamePlay Control Buttons")]
	[SerializeField] private Button[] buttonsRetry;
	[SerializeField] private Button[] buttonsQuit;

	[Space]
	[SerializeField] private InGameEvent gameOverPanel;
	[SerializeField] private InGameEvent hudPanel;
	[SerializeField] private InGameEvent pausedPanel;

	[Header("Game Over specifics")]
	[SerializeField] private TextMeshProUGUI gameOverText;
	[SerializeField] private string gameOverSpiel_TimesUp = "Time's Up";
	[SerializeField] private string gameOverSpiel_Normal = "Game Over";

	[Inject] PlayerStats_Observer playerStats;

	[Inject] DialogueController_Observer dialogueObserver;
	[Inject] Timer_Setter timerSetter;
	[Inject] Timer_Observer timerObserver;

	private void Start() {
		SetUpGamePlayButtons();

		timerObserver.IsCountdownOver()
			.Where(isOver => isOver)
			.Subscribe(_ => OnGameOver(gameOverSpiel_TimesUp))
			.AddTo(this);

		playerStats.IsGameOver()
			.Where(isGameOver => isGameOver)
			.Subscribe(_ => OnGameOver(gameOverSpiel_Normal))
			.AddTo(this);

		dialogueObserver.IsDialogueDone()
			.Subscribe(isDone => {
				if(isDone) {
					hudPanel.FireEvent();
					timerSetter.StartCountdown();
				} else {
					hudPanel.CancelNow();
					timerSetter.PauseCountdown();
				}
			})
			.AddTo(this);

		SetUIStartingState();
	}

	private void OnGameOver(string displayString) {
		gameOverText.text = displayString;
		gameOverPanel.FireEvent();
		hudPanel.CancelNow();
	}

	private void SetUpGamePlayButtons() {
		foreach(Button button in buttonsRetry) {
			button.OnClickAsObservable()
				.Subscribe(_ => {
					PlayerPrefsUtil.ConfigRetryStats();
					SceneUtil.LoadScene(SceneManager.GetActiveScene().buildIndex);
				})
				.AddTo(this);
		}

		foreach(Button button in buttonsQuit) {
			button.OnClickAsObservable()
				.Subscribe(_ => {
					SceneUtil.LoadMainMenu();
				})
				.AddTo(this);
		}
	}

	private void SetUIStartingState() {
		gameOverPanel.CancelNow();

		LogUtil.PrintInfo(gameObject, GetType(), "HudPanel FireEvent...");
		hudPanel.FireEvent();
	}

}

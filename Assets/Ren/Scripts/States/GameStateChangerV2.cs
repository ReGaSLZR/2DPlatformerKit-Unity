using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Zenject;

public class GameStateChangerV2 : MonoBehaviour {

	[Header("GamePlay Control Buttons")]
	[SerializeField] private Button[] buttonsRetry;
	[SerializeField] private Button[] buttonsQuit;

	[Space]
	[SerializeField] private InGameEvent gameOverPanel;
	[SerializeField] private InGameEvent screenInputButtonsPanel;

	[Header("Game Over specifics")]
	[SerializeField] private TextMeshProUGUI gameOverText;
	[SerializeField] private string gameOverSpiel_TimesUp = "Time's Up";
	[SerializeField] private string gameOverSpiel_Normal = "Game Over";

	[Inject] PlayerStats_Observer playerStats;

	[Inject] PlayerInputControls playerInput;
	[Inject] DialogueController_Observer dialogueObserver;
	[Inject] Timer_Observer timerObserver;

	private void Start() {
		SetUpGamePlayButtons();

		timerObserver.IsCountdownOver()
			.Where(isOver => isOver)
			.Subscribe(_ => {
				AnalyticsUtil.RecordLevel_TimesUp();
				OnGameOver(gameOverSpiel_TimesUp); 
			})
			.AddTo(this);

		playerStats.IsGameOver()
			.Where(isGameOver => isGameOver)
			.Subscribe(_ => {
				AnalyticsUtil.RecordLevel_Fail();
				OnGameOver(gameOverSpiel_Normal); 
			})
			.AddTo(this);

		dialogueObserver.IsDialogueDone()
			.Subscribe(isDone => SetScreenInputButtonsEnabled(isDone))
			.AddTo(this);

		SetUIStartingState();
	}

	private void OnGameOver(string displayString) {
		gameOverText.text = displayString;
		gameOverPanel.FireEvent();

		SetScreenInputButtonsEnabled(false);
	}

	private void SetUpGamePlayButtons() {
		foreach(Button button in buttonsRetry) {
			button.OnClickAsObservable()
				.Subscribe(_ => {
					AnalyticsUtil.RecordLevel_Retry();
					PlayerPrefsUtil.ConfigRetryStats();

					SceneUtil.LoadScene(SceneUtil.GetSceneIndex_Current());
				})
				.AddTo(this);
		}

		foreach(Button button in buttonsQuit) {
			button.OnClickAsObservable()
				.Subscribe(_ => {
					AnalyticsUtil.RecordLevel_Quit();
					SceneUtil.LoadMainMenu();
				})
				.AddTo(this);
		}
	}

	private void SetUIStartingState() {
		gameOverPanel.CancelNow();

		LogUtil.PrintInfo(gameObject, GetType(), "HudPanel FireEvent...");
		SetScreenInputButtonsEnabled(true);
	}

	private void SetScreenInputButtonsEnabled(bool isEnabled) {
		if(playerInput.hasScreenButtons) {
			if(isEnabled) {
				screenInputButtonsPanel.FireEvent();
			} else {
				screenInputButtonsPanel.CancelNow();
			}
		}
	}

}

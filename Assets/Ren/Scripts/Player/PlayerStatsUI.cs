using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using TMPro;
using Zenject;

public class PlayerStatsUI : MonoBehaviour
{

	[Header("Avatar")]
	[SerializeField] private RawImage avatar;
	[SerializeField] private Texture avatarActive;
	[SerializeField] private Texture avatarDead;

	[Header("Health")]
	[SerializeField] private RawImage heartContainer1;
	[SerializeField] private RawImage heartContainer2;
	[SerializeField] private RawImage heartContainer3;
	[SerializeField] private Texture heartActive;
	[SerializeField] private Texture heartInactive;

	[Header("Counters")]
	[SerializeField] private TextMeshProUGUI counterLives;
	[SerializeField] private TextMeshProUGUI counterMoney;
	[SerializeField] private TextMeshProUGUI counterShots;
	[SerializeField] private TextMeshProUGUI counterScrolls;

	[Inject] PlayerStats_Observer playerStats;

	private void Start() {
		InitObserver_Avatar();
		InitObserver_Hearts();
		InitObserver_Counters();
	}

	/*********************** Avatar **********************/

	private void InitObserver_Avatar() {
		playerStats.IsGameOver()
			.Subscribe(isGameOver => {
				avatar.texture = (isGameOver) ? avatarDead : avatarActive;
			})
			.AddTo(this);
	}

	/*********************** Counters **********************/

	private void InitObserver_Counters() {
		playerStats.GetLives()
			.Subscribe(value => (counterLives.text = value.ToString()))
			.AddTo(this);

		playerStats.GetMoney()
			.Subscribe(value => (counterMoney.text = value.ToString()))
			.AddTo(this);

		playerStats.GetShots()
			.Subscribe(value => (counterShots.text = value.ToString()))
			.AddTo(this);

		playerStats.GetScrolls()
			.Subscribe(value => (counterScrolls.text = value.ToString()))
			.AddTo(this);
	}

	/*********************** Health **********************/

	private void InitObserver_Hearts() {
		playerStats.GetHealth()
			.Subscribe(health => {
				UpdateHeartContainers(health);
			})
			.AddTo(this);

		playerStats.IsDead()
			.Where(isDead => isDead)
			.Subscribe(_ => DeactivateAllHearts())
			.AddTo(this);
	}

	private void ActivateHeart(RawImage container) {
		container.texture = heartActive;
	}

	private void DeactivateHeart(RawImage container) {
		container.texture = heartInactive;
	}

	private void UpdateHeartContainers(int health) {
		switch(health) {
			case 3: {
				ActivateHeart(heartContainer1);
				ActivateHeart(heartContainer2);
				ActivateHeart(heartContainer3);
				break;
			}
			case 2: {
				ActivateHeart(heartContainer1);
				ActivateHeart(heartContainer2);

				DeactivateHeart(heartContainer3);
				break;
			}
			case 1: {
				ActivateHeart(heartContainer1);

				DeactivateHeart(heartContainer2);
				DeactivateHeart(heartContainer3);
				break;
			}
		}
	}

	private void DeactivateAllHearts() {
		DeactivateHeart(heartContainer1);
		DeactivateHeart(heartContainer2);
		DeactivateHeart(heartContainer3);
	}

	/*********************** xxxx **********************/

}


using System.Collections;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

public class PlayerStats : MonoBehaviour,
							PlayerStats_Observer,
							PlayerStatSetter_Health,
							PlayerStatSetter_Lives,
							PlayerStatSetter_Shots,
							PlayerStatSetter_Money,
							PlayerStatSetter_Scrolls
{

	[Header("Health")]
	[SerializeField] private int healthMax = 3;
	[SerializeField] private int healthStarting = 3;
	[SerializeField] private float delayResetHealth = 2f;
	[SerializeField] private float delayHit = 1f;
	private ReactiveProperty<int> health;
	private ReactiveProperty<bool> isHit;
	private ReactiveProperty<bool> isFinalHit; //same as gameOver trigger but without delays
	private ReactiveProperty<bool> isDead;

	[Header("Lives")]
	[SerializeField] private int livesMax = 99;
	[SerializeField] private int livesStarting = 5;
	private ReactiveProperty<int> lives;
	private ReactiveProperty<bool> isGameOver;

	[Header("Shots")]
	[SerializeField] private int shotsMax = 999;
	[SerializeField] private int shotsStarting = 5;
	private ReactiveProperty<int> shots;
	private ReactiveProperty<bool> isOutOfShots;

	[Header("Money")]
	[SerializeField] private int moneyMax = 9999;
	[SerializeField] private int moneyStarting = 0;
	private ReactiveProperty<int> money;
	private ReactiveProperty<bool> isOutOfMoney;

	[Header("Scrolls")]
	[SerializeField] private int scrollsMax = 3;
	[SerializeField] private int scrollsStarting = 0;
	private ReactiveProperty<int> scrolls;

	private void Awake() {
		InitReactiveProperties_Basic();
		InitReactiveProperties_Derived();
	}

	private void InitReactiveProperties_Basic() {
		int temp = PlayerPrefs.GetInt(PlayerPrefsUtil.KEY_INT_HEALTH, healthStarting);
		health = new ReactiveProperty<int>((temp <= healthMax) ? temp : healthStarting);

		isHit = new ReactiveProperty<bool>(false);
		isFinalHit = new ReactiveProperty<bool>(false);
//		isDead = new ReactiveProperty<bool>(false);

		temp = PlayerPrefs.GetInt(PlayerPrefsUtil.KEY_INT_LIVES, livesStarting);
		lives = new ReactiveProperty<int>((temp <= livesMax) ? temp : livesStarting);
//		isGameOver = new ReactiveProperty<bool>(false);

		temp = PlayerPrefs.GetInt(PlayerPrefsUtil.KEY_INT_SHOTS, shotsStarting);
		shots = new ReactiveProperty<int>((temp <= shotsMax) ? temp : shotsStarting);
//		isOutOfShots = new ReactiveProperty<bool>(false);

		temp = PlayerPrefs.GetInt(PlayerPrefsUtil.KEY_INT_MONEY, moneyStarting);
		money = new ReactiveProperty<int>((temp <= moneyMax) ? temp : moneyStarting);
//		isOutOfMoney = new ReactiveProperty<bool>(false);

		temp = PlayerPrefs.GetInt(PlayerPrefsUtil.KEY_INT_SCROLLS, scrollsStarting);
		scrolls = new ReactiveProperty<int>((temp <= scrollsMax) ? temp : scrollsStarting);

	}

	private void InitReactiveProperties_Derived() {
//		isHit.Where(value => value)
//			.Subscribe(_ => {
//				StartCoroutine(CoDelayResetIsHit());
//			})
//			.AddTo(this);

		isDead = health.Select(value => {
			bool tempIsDead = (value <= 0);
			StartCoroutine(CoDelayResetHealth(tempIsDead));

			return tempIsDead;
		}).ToReactiveProperty();

		isGameOver = isDead.Select(value => {
			return ((value) && (lives.Value == 0));
		}).ToReactiveProperty();

		isOutOfShots = shots.Select(value => {
			return (value <= 0);
		}).ToReactiveProperty();

		isOutOfMoney = money.Select(value => {
			return (value <= 0);
		}).ToReactiveProperty();
			
	}

	private IEnumerator CoDelayResetIsHit() {
		yield return new WaitForSeconds(delayHit);

		isHit.Value = false;
	}

	private IEnumerator CoDelayResetHealth(bool isDead) {
		yield return new WaitForSeconds(delayResetHealth);

		if(isDead && (lives.Value-1 >= 0)) {
			health.Value = healthMax;
			lives.Value--;
		}
	}

	/*********************** PlayerStats_Observer **********************/

	public ReactiveProperty<int> GetHealth() {
		return health;
	}

	public ReactiveProperty<bool> IsHit() {
		return isHit;
	}

	public ReactiveProperty<bool> IsFinalHit() {
		return isFinalHit;
	}

	public ReactiveProperty<bool> IsDead() {
		return isDead;
	}

	public ReactiveProperty<int> GetLives() {
		return lives;
	}

	public ReactiveProperty<bool> IsGameOver() {
		return isGameOver;
	}

	public ReactiveProperty<int> GetShots() {
		return shots;	
	}

	public ReactiveProperty<bool> IsOutOfShots() {
		return isOutOfShots;
	}

	public ReactiveProperty<int> GetMoney() {
		return money;	
	}

	public ReactiveProperty<bool> IsOutOfMoney() {
		return isOutOfMoney;
	}

	public ReactiveProperty<int> GetScrolls() {
		return scrolls;
	}
		
	/*********************** Health **********************/

//	private void CheckFinalHit() {
//		if((lives.Value == 0) && (health.Value == 1)) {
//			isFinalHit.Value = true;
//		}
//	}

	public void AddHealth(int healthToAdd) {
		int tempHealth = (health.Value + healthToAdd);

		health.Value = (tempHealth < healthMax) ? tempHealth : healthMax;
	}

	public void HitOnce() {
		//CheckFinalHit
		if((lives.Value == 0) && (health.Value == 1)) {
			isFinalHit.Value = true;
		}

		health.Value--;
			
		isHit.Value = true;
		StartCoroutine(CoDelayResetIsHit());
	}

	/*********************** Life **********************/

	public void AddLife() {
		if(lives.Value < livesMax) {
			lives.Value++;	
		}
	}

	public void AddLives(int livesToAdd) {
		int tempNewLives = lives.Value + livesToAdd;	

		lives.Value = (tempNewLives < livesMax) ? tempNewLives : livesMax;	
	}

	public void KillOnce() {
		//CheckFinalHit
		if((lives.Value == 0)) {
			isFinalHit.Value = true;
		}

		health.Value = 0;
		isHit.Value = true;

		StartCoroutine(CoDelayResetIsHit());
	}

	public void StealLives(int livesToSteal) {
		lives.Value -= livesToSteal;
	}

	public void EmptyLives() {
		//CheckFinalHit >> inevitable
		isFinalHit.Value = true;

		lives.Value = 0;
		isDead.Value = true;
	}

	/*********************** Shots **********************/

	public void AddShots(int shots) {
		int tempShots = shots + this.shots.Value;

		this.shots.Value = (tempShots < shotsMax) ? tempShots : shotsMax;
	}

	public bool TakeShots(int shotsToTake) {
		int tempValue = (shots.Value-shotsToTake);
		if(tempValue >= 0) {
			shots.Value = tempValue;
			return true;
		}

		return false;
	}

	/*********************** Money **********************/

	public void AddMoney(int money) {
		int tempMoney  = (money + this.money.Value);

		this.money.Value = (tempMoney < moneyMax) ? tempMoney : moneyMax;
	}

	public bool SpendMoney(int money) {
		int tempMoneyLeft = (this.money.Value - money);

		if(tempMoneyLeft >= 0) {
			this.money.Value = tempMoneyLeft;
			return true;
		}

		return false;
	}

	/*********************** Scrolls **********************/

	public void AddScroll() {
		if(scrolls.Value < scrollsMax)  {
			scrolls.Value++;
		}
	}

	public void AddScrolls(int scrollsToAdd) {
		int tempScrolls = scrolls.Value + scrollsToAdd;
		scrolls.Value = (tempScrolls < scrollsMax) ? tempScrolls : scrollsMax;
	}

}
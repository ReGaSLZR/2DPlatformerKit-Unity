using UnityEngine;
using Zenject;

public class InventoryEvent : InGameEvent
{

	[SerializeField] private INVENTORY_OPERATION operation;
	[Space]
	[SerializeField] private OBJECT_TAG itemAffected = OBJECT_TAG.Item;
	[Tooltip("Only for Scroll-type item tag")]
	[SerializeField] private string scrollKey = "09ud";
	[Space]
	[Tooltip("A value of -1 (when paired with operation SUBTRACT) signals zeroing out.")]
	[SerializeField] private int inventoryValue = -1; 

	[Inject] VolumeController_Observer volumeStats;
	[Inject] PlayerStats_Observer stats;
	[Inject] Timer_Observer timer;

	[Inject] PlayerStatSetter_Health setterHealth;
	[Inject] PlayerStatSetter_Lives setterLife;
	[Inject] PlayerStatSetter_Money setterMoney;
	[Inject] PlayerStatSetter_Scrolls setterScrolls;
	[Inject] PlayerStatSetter_Shots setterShots;
	[Inject] Timer_Setter setterTimer;

	private void Awake()
	{
		if(!itemAffected.ToString().StartsWith("Item_")) {
			LogUtil.PrintWarning(gameObject, GetType(), "itemAffected TAG is not Item. Destroying...");
			Destroy(this.gameObject);
		}
	}

	protected override bool FireNow() {
		bool result = true;

		switch(itemAffected) {

			case OBJECT_TAG.Item_Clock : {
				if(operation == INVENTORY_OPERATION.ADD) {
					result = setterTimer.AddToCountdown(inventoryValue);
				} else {
					if(inventoryValue == -1) {
						result = setterTimer.DeductToCountdown(timer.GetCountdown().Value);
					} else {
						result = setterTimer.DeductToCountdown(inventoryValue);
					}
				}

				break;
			}

			case OBJECT_TAG.Item_Health : {
				if(operation == INVENTORY_OPERATION.ADD) {
					setterHealth.AddHealth(inventoryValue);
				} else {
					//NO ZEROING OUT OF HEALTH HERE...
					//automatic HitOnce() call; nothing else
					setterHealth.HitOnce();
				}

				result = true;

				break;
			}

			case OBJECT_TAG.Item_Life : {
				if(operation == INVENTORY_OPERATION.ADD) {
					setterLife.AddLives(inventoryValue);
				} else {
					if(inventoryValue == -1) {
						//ZEROING OUT OF LIVES IS POSSIBLE THO HAHA.. ULTRA TROLL
						setterLife.EmptyLives();
					} else {
						//automatic KillOnce() call; nothing else
						setterLife.KillOnce();
					}
				}

				result = true;

				break;
			}

			case OBJECT_TAG.Item_Money : {
				if(operation == INVENTORY_OPERATION.ADD) {
					setterMoney.AddMoney(inventoryValue);
					result = true;
				} else {
					if(inventoryValue == -1) {
						result = setterMoney.SpendMoney(stats.GetMoney().Value);
					} else {
						result = setterMoney.SpendMoney(inventoryValue);
					}
				}

				break;
			}

			case OBJECT_TAG.Item_Scroll : {
				if(operation == INVENTORY_OPERATION.ADD) {
					setterScrolls.AddScroll(scrollKey); //NOTE: For now, we force the addition of scrolls to be 1 at a time only.
				} else {
					LogUtil.PrintInfo(gameObject, GetType(), "There's no Subtract operation for Item_Scroll inventory.");
				}

				result = true;

				break;
			}

			case OBJECT_TAG.Item_Shots : {
				if(operation == INVENTORY_OPERATION.ADD) {
					setterShots.AddShots(inventoryValue);
					result = true;
				} else {
					if(inventoryValue == -1) {
						result = setterShots.TakeShots(stats.GetShots().Value);
					} else {
						result = setterShots.TakeShots(inventoryValue);
					}
				}

				break;
			}
		}

		PlayerPrefsUtil.SaveStats(volumeStats, stats);

		return result;
	}

	public override void CancelNow() {
		LogUtil.PrintInfo(gameObject, GetType(), "CancelNow() >> there is no cancellation of InventoryEvent");
	}

}

public enum INVENTORY_OPERATION {
	ADD, SUBTRACT
}


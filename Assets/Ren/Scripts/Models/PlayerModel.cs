using UniRx;

public class PlayerModel {

} //TODO end of PlayerModel

/*********************** INTERFACES **********************/

/*********************** Skills **********************/

public interface PlayerSkills_Observer {
	ReactiveProperty<bool> HasAttacked();
}

	
public interface PlayerStats_Observer {

	ReactiveProperty<int> GetHealth();
	ReactiveProperty<bool> IsHit();
	ReactiveProperty<bool> IsFinalHit();
	ReactiveProperty<bool> IsDead();

	ReactiveProperty<int> GetLives();
	ReactiveProperty<bool> IsGameOver();

	ReactiveProperty<int> GetShots();
	ReactiveProperty<bool> IsOutOfShots();

	ReactiveProperty<int> GetMoney();
	ReactiveProperty<bool> IsOutOfMoney();

	ReactiveProperty<int> GetScrolls();

}

//public interface PlayerStats_Setter {
//	void SetStats(PlayerStats stats);
//}

public interface PlayerStatSetter_Health {

	void AddHealth(int healthToAdd);
	void HitOnce();

}

public interface PlayerStatSetter_Lives {

	void AddLife();
	void AddLives(int livesToAdd);
	void KillOnce();
	void StealLives(int livesToSteal);
	void EmptyLives();

}

public interface PlayerStatSetter_Shots {

	void AddShots(int shots);
	bool TakeShots(int shots);

}

public interface PlayerStatSetter_Money {

	void AddMoney(int money);
	bool SpendMoney(int money);

}

public interface PlayerStatSetter_Scrolls {

	void AddScroll(string scrollKey);
//	void AddScrolls(int scrollsToAdd);

}
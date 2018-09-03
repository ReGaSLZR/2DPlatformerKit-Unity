using UniRx;
using UnityEngine;

public class PlayerGroundController : MonoBehaviour,
									  PlayerGround_Observer,
									  PlayerGround_Setter
{

	private ReactiveProperty<bool> rIsGrounded;
	private ReactiveProperty<bool> rIsWallSliding;

	private void Awake() {
		rIsGrounded = new ReactiveProperty<bool>();
		rIsWallSliding = new ReactiveProperty<bool>();
	}

	public ReactiveProperty<bool> IsGrounded() {
		return rIsGrounded;
	}

	public ReactiveProperty<bool> IsWallSliding() {
		return rIsWallSliding;
	}

	public void UpdateGroundType(GroundType type, bool isActive) {
		switch(type) {
			case GroundType.WALL : {
				rIsWallSliding.Value = isActive;
				break;
			}
			case GroundType.FLOOR : {
				rIsGrounded.Value = isActive;
				break;
			}
		}
	}
}

/*********************** ENUMS **********************/

public enum GroundType {
	WALL, FLOOR
}

/*********************** INTERFACES **********************/

public interface PlayerGround_Observer {
	ReactiveProperty<bool> IsGrounded();
	ReactiveProperty<bool> IsWallSliding();
}

public interface PlayerGround_Setter {
	void UpdateGroundType(GroundType type, bool isActive);
}


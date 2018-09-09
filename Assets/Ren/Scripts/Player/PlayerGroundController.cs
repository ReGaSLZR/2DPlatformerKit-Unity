using UniRx;
using UnityEngine;

public class PlayerGroundController : MonoBehaviour,
									  PlayerGround_Observer,
									  PlayerGround_Setter
{

	private ReactiveProperty<bool> rIsHanging;
	private ReactiveProperty<bool> rIsGrounded;
	private ReactiveProperty<bool> rIsWallSliding;
	private ReactiveProperty<WALL_SLIDE_SIDE> rWallSide;

	private void Awake() {
		rWallSide = new ReactiveProperty<WALL_SLIDE_SIDE>(WALL_SLIDE_SIDE.LEFT);

		rIsGrounded = new ReactiveProperty<bool>();
		rIsHanging = new ReactiveProperty<bool>();
		rIsWallSliding = new ReactiveProperty<bool>();
	}

	public ReactiveProperty<bool> IsGrounded() {
		return rIsGrounded;
	}

	public ReactiveProperty<bool> IsHanging() {
		return rIsHanging;
	}

	public ReactiveProperty<bool> IsWallSliding() {
		return rIsWallSliding;
	}

	public ReactiveProperty<WALL_SLIDE_SIDE> GetWallSide() {
		return rWallSide;
	}

	public void UpdateGroundType(GROUND_SIDE type, bool isActive) {
		switch(type) {
			case GROUND_SIDE.BOTTOM : {
				rIsGrounded.Value = isActive;
				break;
			}
			case GROUND_SIDE.TOP : {
				rIsHanging.Value = isActive;
				break;
			}
		}
	}

	public void UpdateWallSlide(WALL_SLIDE_SIDE side, bool isActive) {
		rIsWallSliding.Value = isActive;
		rWallSide.Value = side;
	}

}

/*********************** ENUMS **********************/

public enum GROUND_SIDE {
	BOTTOM, TOP, SIDE
}

public enum WALL_SLIDE_SIDE {
	LEFT, RIGHT
}

/*********************** INTERFACES **********************/

public interface PlayerGround_Observer {
	ReactiveProperty<bool> IsGrounded();
	ReactiveProperty<bool> IsHanging();
	ReactiveProperty<bool> IsWallSliding();
	ReactiveProperty<WALL_SLIDE_SIDE> GetWallSide();
}

public interface PlayerGround_Setter {
	void UpdateGroundType(GROUND_SIDE type, bool isActive);
	void UpdateWallSlide(WALL_SLIDE_SIDE side, bool isActive);
}


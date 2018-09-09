using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

[RequireComponent(typeof(Collider2D))]
public class PlayerGround : MonoBehaviour
{

	[SerializeField] private GROUND_SIDE type;
	[Header("Applicable only to type SIDE")]
	[SerializeField] private WALL_SLIDE_SIDE side;

	[Inject] PlayerGround_Setter groundSetter;

	private void Start() {
		this.OnTriggerEnter2DAsObservable()
			.Where(otherCollider2D => IsGroundSideMet(otherCollider2D))
			.Subscribe(_ => NotifyGroundChanges(true))
			.AddTo(this);

		this.OnTriggerExit2DAsObservable()
			.Where(otherCollider2D => IsGroundSideMet(otherCollider2D))
			.Subscribe(_ => NotifyGroundChanges(false))
			.AddTo(this);
	}

	private void NotifyGroundChanges(bool isActive) {
		switch(type) {
			case GROUND_SIDE.BOTTOM :
			case GROUND_SIDE.TOP : {
				groundSetter.UpdateGroundType(type, isActive);
				break;
			}
			case GROUND_SIDE.SIDE: {
				groundSetter.UpdateWallSlide(side, isActive);
				break;
			}
		}
	}

	private bool IsGroundSideMet(Collider2D collider) {
		if(type == GROUND_SIDE.BOTTOM || type == GROUND_SIDE.TOP) {
			return TagUtil.IsTagWalkable(collider.tag);
		}

		return TagUtil.IsTagSlideable(collider.tag);
	}

}
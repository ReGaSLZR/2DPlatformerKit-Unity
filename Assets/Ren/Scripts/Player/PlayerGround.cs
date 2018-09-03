using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

[RequireComponent(typeof(Collider2D))]
public class PlayerGround : MonoBehaviour
{

	[SerializeField] private GroundType type;

	[Inject] PlayerGround_Setter groundSetter;

	private void Start() {
		this.OnTriggerEnter2DAsObservable()
			.Where(otherCollider2D => IsGroundTypeMet(otherCollider2D))
			.Subscribe(_ => groundSetter.UpdateGroundType(type, true))
			.AddTo(this);

		this.OnTriggerExit2DAsObservable()
			.Where(otherCollider2D => IsGroundTypeMet(otherCollider2D))
			.Subscribe(_ => groundSetter.UpdateGroundType(type, false))
			.AddTo(this);
	}

	private bool IsGroundTypeMet(Collider2D collider) {
		if(type == GroundType.FLOOR) {
			return TagUtil.IsTagWalkable(collider.tag);
		}

		return TagUtil.IsTagSlideable(collider.tag);
	}

}
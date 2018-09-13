using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(Collider2D))]
public class TargetDetector : MonoBehaviour {

	[SerializeField] private OBJECT_TAG[] targetTags;
	[SerializeField] private float range = 5f;
	[Tooltip("If set to FALSE, this will capture ALL targets within its range upon detection." +
		"If set to TRUE, disregard the value of Range.")]
	[SerializeField] private bool isLockedToFirstSingleTarget; 

	public ReactiveProperty<bool> isTargetDetected { get; private set; }
	public List<Collider2D> targets { get; private set; }

	private void Awake() {
		isTargetDetected = new ReactiveProperty<bool>(false);
		targets = new List<Collider2D>();
	}

	private void Start() {
		this.OnTriggerEnter2DAsObservable()
			.Where(otherCollider2D => IsMatchingTag(otherCollider2D.tag))
			.Subscribe(otherCollider2D => {
				if(isTargetDetected.Value == false) {
					RefreshTargets(otherCollider2D);
					isTargetDetected.Value = true;
				}
			})
			.AddTo(this);

		this.OnTriggerExit2DAsObservable()
			.Where(otherCollider2D => IsMatchingTag(otherCollider2D.tag))
			.Subscribe(otherCollider2D => {
				if(isTargetDetected.Value == true) {
					RefreshTargets(otherCollider2D);
					isTargetDetected.Value = false;
				}
			})
			.AddTo(this);

		this.OnCollisionEnter2DAsObservable()
			.Where(otherCollision2D => IsMatchingTag(otherCollision2D.gameObject.tag))
			.Subscribe(otherCollision2D => {
				if(isTargetDetected.Value == false) {
					RefreshTargets(otherCollision2D.collider);
					isTargetDetected.Value = true;
				}
			})
			.AddTo(this);

		this.OnCollisionExit2DAsObservable()
			.Where(otherCollision2D => IsMatchingTag(otherCollision2D.gameObject.tag))
			.Subscribe(otherCollision2D => {
				if(isTargetDetected.Value == true) {
					RefreshTargets(otherCollision2D.collider);
					isTargetDetected.Value = false;
				}
			})
			.AddTo(this);
	}

	private void RefreshTargets(Collider2D detectedCollider) {
		if(isLockedToFirstSingleTarget && IsMatchingTag(detectedCollider.tag)) {
			targets.Clear();
			targets.Add(detectedCollider);
		}
		else {
			Collider2D[] tempTargets = Physics2D.OverlapCircleAll(transform.position, range);	
			targets.Clear();
			//filter targets by tags

			foreach(Collider2D collider2D in tempTargets) {
				if(IsMatchingTag(collider2D.tag)) {
					targets.Add(collider2D);
				}	
			}
		}
	}

	private bool IsMatchingTag(string tag) {
		for(int x=0; x<targetTags.Length; x++) {
			if(tag.Equals(targetTags[x].ToString())) {
				return true;
			}
		}

		return false;
	}

}

using UnityEngine;
using UnityEngine.Playables;

public class PlayableEvent : InGameEvent
{

	[SerializeField] private PlayableDirector _playableDirector;

	private void Awake() {
		if(_playableDirector == null) {
			LogUtil.PrintInfo(gameObject, GetType(), "No PlayableDirector set. Destroying...");
			Destroy(this.gameObject);
		}

		_playableDirector.playOnAwake = false;
		autoCancelDelay = (float) _playableDirector.duration;
	}

	protected override bool FireNow() {
		_playableDirector.Play();
		return true;
	}

	public override void CancelNow() {
		_playableDirector.Stop();
	}

}
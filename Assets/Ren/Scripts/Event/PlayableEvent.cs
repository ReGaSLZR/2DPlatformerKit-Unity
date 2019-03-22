using UnityEngine;
using UnityEngine.Playables;

public class PlayableEvent : InGameEvent
{

	[SerializeField] private PlayableDirector _playableDirector;
	[Tooltip("For the purpose of closing the main _playableDirector.")]
	[SerializeField] private PlayableDirector _cancelPlayableDirector;

	private void Awake() {
		if(_playableDirector == null) {
			LogUtil.PrintInfo(gameObject, GetType(), "No PlayableDirector set. Destroying...");
			Destroy(this.gameObject);
		}

		_playableDirector.playOnAwake = false;
		autoCancelDelay = (float) _playableDirector.duration;
	}

	protected override bool FireNow() {
		if(_cancelPlayableDirector != null) {
			_cancelPlayableDirector.Stop();
		}

		_playableDirector.Play();
		return true;
	}

	public override void CancelNow() {
		_playableDirector.Stop();

		if(_cancelPlayableDirector != null) {
			_cancelPlayableDirector.Play();	
		}
	}

	public float GetMainPlayableDuration() {
		return (float) _playableDirector.duration;
	}

	public float GetCancelPlayableDuration() {
		return (_cancelPlayableDirector == null) ? 1f : (float) _cancelPlayableDirector.duration;
	}

}
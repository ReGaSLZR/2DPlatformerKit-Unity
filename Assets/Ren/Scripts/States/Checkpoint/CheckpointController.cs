using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CheckpointController : MonoBehaviour,
									Checkpoint_Getter,
									Checkpoint_Setter
{

	[SerializeField] private GameObject startLocation;
	[SerializeField] private AudioClip[] clipsOnReach;

	private Vector3 currentCheckpoint;

	private AudioSource _audioSource;

	private void Awake() {
		_audioSource = GetComponent<AudioSource>();
		currentCheckpoint = startLocation.transform.position;
	}

	public void SetCheckpoint(Vector3 location, string checkPointName) {
		currentCheckpoint = location;

		AudioUtil.PlayRandomClip(GetType(), clipsOnReach, _audioSource);

		LogUtil.PrintInfo(gameObject, GetType(), "Checkpoint " + checkPointName + " reached!");
	}

	public Vector3 GetCurrentCheckpoint() {
		return currentCheckpoint;
	}

}

/* 		INTERFACES		*/

public interface Checkpoint_Getter {
	Vector3 GetCurrentCheckpoint();
}

public interface Checkpoint_Setter {
	void SetCheckpoint(Vector3 location, string checkPointName);
}
using UnityEngine;

public abstract class KillableBehaviour : MonoBehaviour {

	public abstract void DisableMovement();
	public abstract void EnableMovement();

	public abstract void HitOnce();
	public abstract void KillOnce();

}

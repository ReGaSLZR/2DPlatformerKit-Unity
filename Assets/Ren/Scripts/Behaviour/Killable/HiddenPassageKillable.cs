using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class HiddenPassageKillable : KillableBehaviour {

	[Header("Blocker Sprites [Normal State First]")]
	[SerializeField] private GameObject[] blockerStates;
	[Space]
	[SerializeField] private GameObject prefabDestroyFX;
	[Space]
	[SerializeField] private AudioClip clipHit;

	private int hits = 0; //max count == blockerStates.Length
	private AudioSource _audioSource;

	[Inject] Instantiator instantiator;

	private void Awake() {
		if(blockerStates == null || blockerStates.Length == 0) {
			LogUtil.PrintWarning(gameObject, GetType(), "No blockerStates defined. Destroying...");
			Destroy(this);
		}

		_audioSource = GetComponent<AudioSource>();
		ActivateNextState();	
	}

	private void ActivateNextState() {
		for(int x=(blockerStates.Length-1); x>=0; x--) {
			if(hits == x) {
				blockerStates[x].gameObject.SetActive(true);	
			}
			else {
				blockerStates[x].gameObject.SetActive(false);
			}
		}
	}

	private void DestroyBlocker() {
		if(prefabDestroyFX != null) {
			instantiator.InjectPrefab(Instantiate(prefabDestroyFX, gameObject.transform.position,
													gameObject.transform.rotation));
		}

		Destroy(this.gameObject);
	}

	public override void DisableMovement() {
		//TODO ren
		LogUtil.PrintWarning(gameObject, GetType(), "DisableMovement() not implemented");
	}

	public override void EnableMovement() {
		//TODO ren
		LogUtil.PrintWarning(gameObject, GetType(), "EnableMovement() not implemented");
	}

	public override void HitOnce() {
		hits++;

		if(hits < blockerStates.Length) { 
			ActivateNextState();
			AudioUtil.PlaySingleClip(GetType(), clipHit, _audioSource);
		} 
		else { 
			DestroyBlocker(); 
		}
	}

	public override void KillOnce() {
		DestroyBlocker();
	}

}

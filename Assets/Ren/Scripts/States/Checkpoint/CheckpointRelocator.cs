using System.Collections;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class CheckpointRelocator : MonoBehaviour 
{

	[SerializeField] private GameObject prefabRelocationFX;
	[SerializeField] private float relocationDelay = 1f;
	[SerializeField] private AudioClip[] clipsOnRevival;

	[Inject] Checkpoint_Getter checkpointGetter;
	[Inject] PlayerStats_Observer playerStats;
	[Inject] Instantiator instantiator;

	private AudioSource _audioSource;

	private void Awake() {
		_audioSource = GetComponent<AudioSource>();
	}

	private void Start() {
		playerStats.IsDead()
			.Where(isDead => (!playerStats.IsFinalHit().Value) && isDead)
			.Subscribe(_ => StartCoroutine(CorRelocate()))
			.AddTo(this);

		RelocateNow(); //NOTE: to force the gameObject to move to the first checkpoint already
	}

	private void RelocateNow() {
		this.gameObject.transform.position = checkpointGetter.GetCurrentCheckpoint();
	}

	private IEnumerator CorRelocate() {

		yield return new WaitForSeconds(relocationDelay);
		RelocateNow();

		if(prefabRelocationFX != null) {
			GameObject relocationFX = Instantiate(prefabRelocationFX, 
				this.gameObject.transform.position, this.gameObject.transform.rotation);
			instantiator.InjectPrefab(relocationFX);
		}	
			
		AudioUtil.PlayRandomClip(GetType(), clipsOnRevival, _audioSource);
	}

}

using UnityEngine;
using System.Collections;

public class EmitPatrol : MonoBehaviour
{

	[SerializeField] private float delay;
	[SerializeField] private float duration;

	private void Awake()
	{
	
	}

	private void Start()
	{
	
	}

	private IEnumerator CorEmit() {
		yield return new WaitForSeconds(delay);
		StartEmitting();

		yield return new WaitForSeconds(duration);
		StopEmitting();
	}

	private void StartEmitting() {
		
	}

	private void StopEmitting() {
		
	}

}


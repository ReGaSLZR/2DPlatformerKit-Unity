using UnityEngine;
using Zenject;

public class QuakeOnAwake : MonoBehaviour {

	[Inject] CameraShake_Setter camShaker;

	private void Start () {
		camShaker.StartShaking();	
	}

}

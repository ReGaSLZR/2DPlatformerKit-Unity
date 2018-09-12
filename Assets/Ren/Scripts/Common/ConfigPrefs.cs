using UnityEngine;

public class ConfigPrefs : MonoBehaviour {

	[Header("Audio config")]
	[Range(0f, 1f)]
	[SerializeField] private float defaultVolumeBGM = 1f;
	[Range(0f, 1f)]
	[SerializeField] private float defaultVolumeSFX = 1f;
	[SerializeField] private bool isMutedStart = false;

	[Header("Player Starting Stats config")]
	[SerializeField] private int health = 3;
	[SerializeField] private int lives = 5;
	[SerializeField] private int shots = 0;
	[SerializeField] private int money = 0;
	[SerializeField] private int scrolls = 0;

	private void Awake() {
		//NOTE: the following 2 lines are only for dev testing --Ren
//		PlayerPrefs.DeleteAll();
//		PlayerPrefs.Save();

		PlayerPrefsUtil.ConfigFirstRun(isMutedStart, defaultVolumeBGM, defaultVolumeSFX, health, lives, shots, money, scrolls);
	}

}

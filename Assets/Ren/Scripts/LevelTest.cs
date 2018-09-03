using UnityEngine;

public class LevelTest : MonoBehaviour {

	private void Awake () {
		LogUtil.PrintInfo(gameObject, GetType(), "I'm ALIVE!");	
	}

}

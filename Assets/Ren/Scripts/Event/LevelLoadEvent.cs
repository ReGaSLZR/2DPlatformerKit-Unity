using UnityEngine;

public class LevelLoadEvent : InGameEvent {

	[SerializeField] private LOAD_REQUIREMENT requirement = LOAD_REQUIREMENT.LEVEL_CLEAR;
	[SerializeField] private int scrollCount;
	[Tooltip("Refers to the index of the Scene on Build Settings")]
	[SerializeField] private int levelIndex;

	private void OnEnable() {
		if((requirement == LOAD_REQUIREMENT.LEVEL_CLEAR) && (levelIndex <= 0)) {
			LogUtil.PrintWarning(gameObject, GetType(), "Level Index defined is not valid. Destroying...");
			Destroy(this);
		}
	}
	
	protected override bool FireNow() {
		bool result = false;

//		switch(requirement) {

//			case LOAD_REQUIREMENT.LEVEL_CLEAR : {
				if((levelIndex - SceneUtil.GetLatestLevel()) <= 1) {
					LogUtil.PrintInfo("LevelLoadEvent: index to load is: " + levelIndex);
					SceneUtil.LoadScene(levelIndex);
					result = true;
				}

				result = false;
//				break;
//			}
//
//			case LOAD_REQUIREMENT.SCROLL_COUNT : {
//				int scrollCount = PlayerPrefs.GetInt(ConfigPrefs.KEY_INT_SCROLLS, 0);
//				if(scrollCount >= levelIndex) {
//					SceneUtil.LoadScene(levelIndex);
//					result = true;
//				}
//
//				result = false;
//				break;
//			}
//
//		}

		return result;
	}

	public override void CancelNow() {
		LogUtil.PrintInfo(gameObject, GetType(), "Sorry, there are no cancelNow implementations for this class.");
	}

}

public enum LOAD_REQUIREMENT {
	LEVEL_CLEAR,
	SCROLL_COUNT
}

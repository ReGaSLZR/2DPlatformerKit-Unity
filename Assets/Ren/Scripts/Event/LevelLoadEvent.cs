using UnityEngine;

public class LevelLoadEvent : InGameEvent {

	[Tooltip("Refers to the index of the Scene on Build Settings.")]
	[SerializeField] private int levelIndex;

	private void OnEnable() {
		if(levelIndex <= SceneUtil.GetLatestLevel()) {
			LogUtil.PrintWarning(gameObject, GetType(), "Level Index defined is not valid. Destroying...");
			Destroy(this);
		}	
	}
	
	protected override bool FireNow() {
		if((levelIndex - SceneUtil.GetLatestLevel()) <= 1) {
			SceneUtil.LoadScene(levelIndex);
			return true;
		}

		return false;
	}

	public override void CancelNow() {
		LogUtil.PrintInfo(gameObject, GetType(), "Sorry, there are no cancelNow implementations for this class.");
	}

}

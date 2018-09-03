using UnityEngine;

public class LevelLoadEvent : InGameEvent {

	[Tooltip("Refers to the index of the Scene on Build Settings.")]
	[SerializeField] private int levelIndex;

	private void Awake() {
		if(levelIndex <= 0) {
			LogUtil.PrintError(gameObject, GetType(), "Level Index defined is not valid. Destroying...");
			Destroy(this);
		}	
	}
	
	protected override bool FireNow() {
		SceneIndexes.LoadScene(levelIndex);
		return true;
	}

	public override void CancelNow() {
		LogUtil.PrintInfo(gameObject, GetType(), "Sorry, there are no cancelNow implementations for this class.");
	}

}

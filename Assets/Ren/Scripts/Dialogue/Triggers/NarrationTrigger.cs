using UnityEngine;

public class NarrationTrigger : DialogueTrigger {

	[Space]
	[Tooltip("Set the texture of the character talking in this DialogueTrigger.")]
	[SerializeField] private Texture narratorAvatar;

	[Space]
	[SerializeField] private AudioClip openingClip;

	[Space]
	[TextArea(3, 10)]
	[SerializeField] private string[] lines;

	private void Awake() {
		if((lines == null) || (lines.Length == 0)) {
			LogUtil.PrintWarning(gameObject, GetType(), "No narration dialogue lines set.");
			Destroy(this);
		}
			
	}

	protected override void StartDialogue() {
		LogUtil.PrintInfo(gameObject, GetType(), "StartDialogue()...");
		dialogueSetter.StartNarration(narratorAvatar, lines, openingClip);
	}

}

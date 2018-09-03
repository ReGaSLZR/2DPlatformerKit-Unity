using UnityEngine;

public class ConversationTrigger : DialogueTrigger {

	[SerializeField] private ConversationLine[] lines;

	private string[] stringLines;
	private Texture[] textureAvatars;
	private AudioClip[] clipLines;

	private void Awake() {
		if((lines == null) || (lines.Length == 0)) {
			LogUtil.PrintWarning(gameObject, GetType(), "No convo dialogue lines set.");
			Destroy(this);
		}

		ConfigLines();
		ConfigClipLines();
		ConfigTextures();
	}
	
	protected override void StartDialogue() {
		dialogueSetter.StartConversation(textureAvatars, stringLines, clipLines);
	}

	private void ConfigLines() {
		stringLines = new string[lines.Length];

		for(int x=0; x<lines.Length; x++) {
			stringLines[x] = lines[x].line;	
		}
	}

	private void ConfigClipLines() {
		clipLines = new AudioClip[lines.Length];

		for(int x=0; x<lines.Length; x++) {
			clipLines[x] = lines[x].clipLine;	
		}
	}

	private void ConfigTextures() {
		textureAvatars = new Texture[lines.Length];

		for(int x=0; x<lines.Length; x++) {
			textureAvatars[x] = lines[x].speakerAvatar;	
		}
	}

}

[System.Serializable]
public class ConversationLine {

	[TextArea(3, 10)]
	public string line;
	public Texture speakerAvatar;
	public AudioClip clipLine;

}

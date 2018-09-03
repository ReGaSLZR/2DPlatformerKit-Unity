using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class DialogueController : MonoBehaviour, 
								  DialogueController_Observer,
								  DialogueController_Setter
{

	[SerializeField] private RawImage dialoguePanel;
	[Space]
	[SerializeField] private PlayableDirector playOnStart;
	[SerializeField] private PlayableDirector playOnEnd;
	[Space]
	[SerializeField] private DialogueLineFeeder feeder;
	[Space]
	[SerializeField] private Button buttonSkip;
	[SerializeField] private TextMeshProUGUI buttonText;
	[Space]
	[SerializeField] private RawImage avatar;

	[Header("Dialogue Focus")]
	[SerializeField] private CinemachineVirtualCamera focusCam;

	private AudioSource _audioSource;
	private ReactiveProperty<bool> isFinished;

	private int currentLineIndex = 0;

	private string[] lines;
	private Texture[] avatarTextures;
	private AudioClip[] lineClips;

	private string buttonTextNext;
	private string buttonTextFinish;

	private void Awake() {
		_audioSource = GetComponent<AudioSource>();
		isFinished = new ReactiveProperty<bool>(true);

		DisableFocusCam();
		DeactivateAllPlayables();
	}

	private void DisableFocusCam() {
		focusCam.gameObject.SetActive(false);
	}

	private void Start() {
		feeder.isDone.Where(done => done)
			.Subscribe(_ => OnFinishCurrentLine())
			.AddTo(this);

		if(buttonSkip != null) {
			buttonSkip.OnClickAsObservable()
				.Subscribe(_ => OnFinishCurrentLine())
				.AddTo(this);
		}
	}

	private void DeactivateAllPlayables() {
		DisablePlayable(playOnStart);
		DisablePlayable(playOnEnd);

		dialoguePanel.gameObject.SetActive(false);
	}

	private void OnFinishCurrentLine() {
		if((currentLineIndex+1) < lines.Length) { //NOTE: if there is a next line...
			currentLineIndex++;

			feeder.FeedLine(lines[currentLineIndex]);
			avatar.texture = avatarTextures[currentLineIndex];
			AudioUtil.PlaySingleClip(GetType(), lineClips[currentLineIndex], _audioSource);

			SetSkipButtonText();
		} else {
			StopAllCoroutines();
			StartCoroutine(CorFinishDialogue());
		}
	}

	private IEnumerator CorCommenceDialogue() {
		currentLineIndex = 0;

		avatar.texture = avatarTextures[currentLineIndex];
		dialoguePanel.gameObject.SetActive(true);
		isFinished.Value = false;

		SetSkipButtonText();

		EnablePlayable(playOnStart);
		yield return new WaitForSeconds((float) playOnStart.duration);

		feeder.FeedLine(lines[currentLineIndex]);
		AudioUtil.PlaySingleClip(GetType(), lineClips[currentLineIndex], _audioSource);
	}

	private IEnumerator CorFinishDialogue() {
		feeder.FeedLine(" ");
		EnablePlayable(playOnEnd);
		yield return new WaitForSeconds((float) playOnEnd.duration);

		isFinished.Value = true;
		feeder.StopOngoingFeed();
		DeactivateAllPlayables();
		DisableFocusCam();
	}

	private void SetSkipButtonText() {
		buttonText.SetText( (currentLineIndex == (lines.Length - 1)) ? buttonTextFinish : buttonTextNext );
	}

	private void DisablePlayable(PlayableDirector playable) {
		playable.gameObject.SetActive(false);
		playable.Stop();
	}

	private void EnablePlayable(PlayableDirector playable) {
		playable.gameObject.SetActive(true);
		playable.Play();
	}

	private void ActualStartDialogue() {
		LogUtil.PrintInfo(gameObject, GetType(), "Starting Dialogue...");

		StopAllCoroutines();
		StartCoroutine(CorCommenceDialogue());
	}

	public ReactiveProperty<bool> IsDialogueDone() {
		return isFinished;
	}

	public void ConfigButtonText(string buttonTextNext, string buttonTextFinish) {
		this.buttonTextNext = buttonTextNext;
		this.buttonTextFinish = buttonTextFinish;
	}

	public void ConfigFocusedObject(Transform focusedObject) {
		if(focusedObject != null) {
			focusCam.m_Follow = focusedObject;
			focusCam.gameObject.SetActive(true);
		}
	}

	public void StartNarration(Texture avatarTexture, string[] lines, AudioClip openingClip) {
		avatarTextures = new Texture[lines.Length];
		for(int x=0; x<lines.Length; x++) {
			avatarTextures[x] = avatarTexture;
		}

		this.lines = lines;

		lineClips = new AudioClip[lines.Length];
		lineClips[0] = openingClip;

		ActualStartDialogue();
	}

	public void StartConversation(Texture[] avatarTextures, string[] lines, AudioClip[] lineClips) {
		this.avatarTextures = avatarTextures;
		this.lines = lines;
		this.lineClips = lineClips;

		ActualStartDialogue();
	}

	public void StopDialogue() {
		LogUtil.PrintInfo(gameObject, GetType(), "Stopping Dialogue...");

		StopAllCoroutines();
		StartCoroutine(CorFinishDialogue());
	}

}
	
public interface DialogueController_Observer {

	ReactiveProperty<bool> IsDialogueDone();

}

public interface DialogueController_Setter {

	void ConfigFocusedObject(Transform focusedObject);

	void ConfigButtonText(string buttonTextNext, string buttonTextFinish);

	void StartNarration(Texture avatarTexture, string[] lines, AudioClip openingClip);

	void StartConversation(Texture[] avatarTextures, string[] lines, AudioClip[] lineClips);

	void StopDialogue();

}
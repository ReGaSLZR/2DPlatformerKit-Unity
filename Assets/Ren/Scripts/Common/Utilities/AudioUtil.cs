using UnityEngine;

public class AudioUtil {

	public static void PlayRandomClip(System.Type logger, AudioClip[] clips, AudioSource _audioSource) {
		if((clips != null) && (clips.Length > 0)) {
			PlayClip(clips[Random.Range(0, clips.Length)], _audioSource);
		}
		else {
			LogUtil.PrintWarning(_audioSource.gameObject, logger, "Null or empty AudioClips supplied.");
		}
	}

	public static void PlaySingleClip(System.Type logger, AudioClip clip, AudioSource _audioSource) {
		if((clip != null) && (_audioSource != null)) {
			PlayClip(clip, _audioSource);
		}
		else {
			LogUtil.PrintWarning(_audioSource.gameObject, logger, "Null AudioClip or AudioSource supplied.");
		}
	}

	private static void PlayClip(AudioClip clip, AudioSource _audioSource) {
		_audioSource.Stop();
		_audioSource.clip = clip;
		_audioSource.Play();
	}

}

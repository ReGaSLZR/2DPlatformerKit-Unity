using System.Collections.Generic;
using UnityEngine.Analytics;

public class AnalyticsUtil {

	private static string KEY_FIRST_GAME_RUN = "KEY_FIRST_GAME_RUN";
	private static string KEY_CLEAR_LEVEL = "KEY_CLEAR_LEVEL";
	private static string KEY_QUIT_LEVEL = "KEY_QUIT_LEVEL";
	private static string KEY_RETRY_LEVEL = "KEY_RETRY_LEVEL";

	private static string KEY_FAIL_LEVEL = "KEY_FAIL_LEVEL"; //normal GameOver screen
	private static string KEY_TIMES_UP_LEVEL = "KEY_TIMES_UP_LEVEL"; //Time's Up-type GameOver screen

	private static void PingAnalytics(string key, IDictionary<string, object> eventData) {
		AnalyticsEvent.Custom(key, eventData);
	}

	private static IDictionary<string, object> MakeLevelData(/* TODO add  params here */) {
		IDictionary<string, object> data = new Dictionary<string, object>();
		data.Add("Level Index", SceneUtil.GetSceneIndex_Current());
		data.Add("Level Name", SceneUtil.GetSceneName_Current());
		/* TODO add more data here */

		return data;
	}

	public static void RecordFirstRun() {
		PingAnalytics(KEY_FIRST_GAME_RUN, null);
	}

	public static void RecordLevel_Clear() {
		PingAnalytics(KEY_CLEAR_LEVEL, MakeLevelData());
	}

	public static void RecordLevel_Quit() {
		PingAnalytics(KEY_QUIT_LEVEL, MakeLevelData());
	}

	public static void RecordLevel_Retry() {
		PingAnalytics(KEY_RETRY_LEVEL, MakeLevelData());
	}

	public static void RecordLevel_Fail() {
		PingAnalytics(KEY_FAIL_LEVEL, MakeLevelData());
	}

	public static void RecordLevel_TimesUp() {
		PingAnalytics(KEY_TIMES_UP_LEVEL, MakeLevelData());
	}

}


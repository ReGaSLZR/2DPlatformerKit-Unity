using UnityEngine;

public class LogUtil {

	private const string CONCATENATOR = ": ";

	public static void PrintInfo(Object gameObject, System.Type logger, string message) {
		Debug.Log(logger.Name.ToString() + CONCATENATOR + message, gameObject);
	}

	public static void PrintWarning(Object gameObject, System.Type logger, string message) {
		Debug.LogWarning(logger.Name + CONCATENATOR + message, gameObject);
	}

	public static void PrintError(Object gameObject, System.Type logger, string message) {
		Debug.LogError(logger.Name + CONCATENATOR + message, gameObject);
	}

	/* 
	 	no Context / GameObject reference;
		good for non-MonoBehaviour accessors
	 */

	public static void PrintInfo(System.Type logger, string message) {
		Debug.Log(logger.Name.ToString() + CONCATENATOR + message);
	}

	public static void PrintWarning(System.Type logger, string message) {
		Debug.LogWarning(logger.Name + CONCATENATOR + message);
	}

	public static void PrintError(System.Type logger, string message) {
		Debug.LogError(logger.Name + CONCATENATOR + message);
	}

	public static void PrintInfo(string message) {
		Debug.Log(message);
	}

	public static void PrintWarning( string message) {
		Debug.LogWarning(message);
	}

	public static void PrintError(string message) {
		Debug.LogError(message);
	}

}

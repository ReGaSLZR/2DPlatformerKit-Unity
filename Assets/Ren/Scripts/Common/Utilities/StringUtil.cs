using UnityEngine;

public class StringUtil 
{
	private const int SCROLL_KEY_LENGTH = 4;
	private const string GLYPHS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

	public static bool IsNonNullNonEmpty(string param) {
		return ((param != null) && (param.Length > 0));
	}

	public static string GetRandomScrollKey() {
		char[] randomChars = new char[SCROLL_KEY_LENGTH];

		for(int x=0; x<SCROLL_KEY_LENGTH; x++) {
			randomChars[x] = GLYPHS[Random.Range(0, GLYPHS.Length)];
		}

		return new string(randomChars);
	}

	public static string CreateScrollKey(int instanceId) {
		return (instanceId < 0) ? (instanceId*-1).ToString() : instanceId.ToString();
	}

}

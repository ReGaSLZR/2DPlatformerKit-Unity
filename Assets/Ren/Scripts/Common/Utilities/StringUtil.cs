public class StringUtil 
{

	public static bool IsNonNullNonEmpty(string param) {
		return ((param != null) && (param.Length > 0));
	}

}

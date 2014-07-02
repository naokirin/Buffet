using System;

public sealed class TypeTranslation
{
	public static string TranslateDBType(Type t)
	{
		if (t == typeof(int)) return "INTEGER";
		return "VARCHAR(36)";
	}
}

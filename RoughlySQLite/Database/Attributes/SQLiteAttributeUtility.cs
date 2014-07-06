using System;

namespace RoughlySQLite
{
	static class SQLiteAttributeUtility
	{
		public static Attribute GetAttribute(this Type t, Type attributeType)
		{
			return Attribute.GetCustomAttribute(
				t, attributeType, true);
		}

		public static Attribute[] GetAttributes(this Type t, Type attributeType)
		{
			return Attribute.GetCustomAttributes(
				t, attributeType, true);
		}
	}
}


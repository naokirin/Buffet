using System;
using System.IO;
using System.Runtime.Serialization;

namespace RoughlySQLite
{

	enum SQLiteType
	{
		Integer,
		Bigint,
		Real,
		Text,
		Serialized,
		DateTime,
		Blob
	}

	static class TypeTranslation
	{
		public static SQLiteType ToSQLiteType(this Type t)
		{
			if (t == typeof(Boolean) || t == typeof(Byte) || t == typeof(UInt16)
			    || t == typeof(SByte) || t == typeof(Int16) || t == typeof(Int32)
			    || t.IsEnum)
			{
				return SQLiteType.Integer;
			}
			else if (t == typeof(UInt32) || t == typeof(Int64))
			{
				return SQLiteType.Bigint;
			}
			else if (t == typeof(Single) || t == typeof(Double) || t == typeof(Decimal))
			{
				return SQLiteType.Real;
			}
			else if (t == typeof(DateTime))
			{
				return SQLiteType.DateTime;
			}
			else if (t == typeof(byte[]))
			{
				return SQLiteType.Blob;
			}
			else if (t == typeof(String) || t == typeof(Guid))
			{
				return SQLiteType.Text;
			}
			else if (t.IsSerializable)
			{
				return SQLiteType.Serialized;
			}

			throw new NotSupportedException("Unknown to translate " + t.FullName + " to SQLite type.");
		}

		public static string ToSQLiteTypeString(this Type t)
		{
			switch(t.ToSQLiteType())
			{
			case SQLiteType.Integer:
				return "INTEGER";
			case SQLiteType.Bigint:
				return "BIGINT";
			case SQLiteType.Real:
				return "REAL";
			case SQLiteType.Text:
			case SQLiteType.Serialized:
				return "TEXT";
			case SQLiteType.DateTime:
				return "DATETIME";
			case SQLiteType.Blob:
				return "BLOB";
			}
			throw new NotSupportedException("Unknown to translate " + t.FullName + " to SQLite type.");
		}
	}
}

using System.IO;
using RoughlySQLite;
using System;

#if NET45
using SQLiteConnection = System.Data.SQLite.SQLiteConnection;
using SQLiteException = System.Data.SQLite.SQLiteException;
#else
using SQLiteConnection = Mono.Data.Sqlite.SqliteConnection;
using SQLiteException = Mono.Data.Sqlite.SqliteException;
#endif

namespace RoughlySQLiteTest
{
	public static class DbTest
	{
		public static string GetTempFileName()
		{
			#if NETFX_CORE
			var name = Guid.NewGuid () + ".sqlite";
			return Path.Combine (Windows.Storage.ApplicationData.Current.LocalFolder.Path, name);
			#else
			return Path.GetTempFileName();
			#endif
		}
	}


	class OnlyPrimaryKeyTable
	{
		[PrimaryKey]
		public string PKey { get; set; }
	}

	class AutoIncrementColumnTable
	{
		[PrimaryKey(IsAutoIncrement = true)]
		public int PKey { get; set; }
	}

	class AutoIncrementColumnWithOthersTable
	{
		[PrimaryKey(IsAutoIncrement = true)]
		public int PKey { get; set; }

		public string TestColumn { get; set; }
	}

	class AutoIncrementColumnWithStringTable
	{
		[PrimaryKey(IsAutoIncrement = true)]
		public string PKey { get; set; }
	}

	[MultiColumnPrimaryKey(new string[]{ "PKey1", "PKey2" })]
	class MultiColumnPrimaryKeyTable
	{
		public string PKey1 { get; set; }

		public string PKey2 { get; set; }
	}

	class InvalidMultiColumnPrimaryKeyTable
	{
		[PrimaryKey]
		public string PKey1 { get; set; }

		[PrimaryKey]
		public string PKey2 { get; set; }
	}

	class SpecifiedForeignKeyTable
	{
		[ForeignKey(typeof(OnlyPrimaryKeyTable), "PKey")]
		public string PKey { get; set; }
	}

	[MultiColumnForeignKey(new string[]{ "PKey1", "PKey2" }, typeof(MultiColumnPrimaryKeyTable), new string[] { "PKey1", "PKey2" })]
	class SpecifiedMultiColumnForeignKeyTable
	{
		public string PKey1 { get; set; }
		public string PKey2 { get; set; }
	}

	[MultiColumnForeignKey(new string[]{ "PKey1", "PKey2" }, typeof(MultiColumnPrimaryKeyTable), new string[] { "PKey1", "PKey2" })]
	[MultiColumnForeignKey(new string[]{ "PKey3", "PKey4" }, typeof(MultiColumnPrimaryKeyTable), new string[] { "PKey1", "PKey2" })]
	class SpecifiedMultiColumnForeignKeysTable
	{
		public string PKey1 { get; set; }
		public string PKey2 { get; set; }
		public string PKey3 { get; set; }
		public string PKey4 { get; set; }
	}

	class DateTimeColumnTable
	{
		public DateTime DateTime { get; set; }
	}

	[Serializable]
	class SerializableData
	{
		public int Id = 1;
		public string Str = "test";
	}
	class SerializedColumnTable
	{
		public SerializableData Data { get; set; }
	}

	class BlobColumnTable
	{
		public byte[] Bytes { get; set; }
	}

	enum ColumnTestEnum
	{
		TypeA,
		TypeB,
		TypeC
	}

	class EnumColumnTable
	{
		public ColumnTestEnum Enum { get; set; }
	}

	[Check("ID > 0")]
	class CheckConstraintTable
	{
		public uint ID { get; set; }
	}
}

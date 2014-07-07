using NUnit.Framework;
using RoughlySQLite;

#if NET45
using SQLiteConnection = System.Data.SQLite.SQLiteConnection;
using SQLiteException = System.Data.SQLite.SQLiteException;
#else
using SQLiteConnection = Mono.Data.Sqlite.SqliteConnection;
using SQLiteException = Mono.Data.Sqlite.SqliteException;
#endif

namespace RoughlySQLiteTest
{
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

	[TestFixture]
	public class CreateTableTest
	{
		string dbFileName;
		SQLiteConnectionProvider provider;

		[TestFixtureSetUp]
		public void SetUpFixture()
		{
			dbFileName = DbTest.GetTempFileName();
		}

		[SetUp]
		public void SetUp()
		{
			provider = new SQLiteConnectionProvider(dbFileName);
		}

		[TearDown]
		public void TearDown()
		{
			SQLiteConnectionProvider.DeleteDBFile(dbFileName);
		}

		[Test]
		public void TestCreateOnlyPrimaryKeyTable()
		{
			using(var connection = provider.GetOpenConnection())
			{
				Assert.DoesNotThrow(() => connection.CreateTable<OnlyPrimaryKeyTable>());
			}
		}

		[Test]
		public void TestCreateAutoIncrementColumnTable()
		{
			using(var connection = provider.GetOpenConnection())
			{
				Assert.DoesNotThrow(() => connection.CreateTable<AutoIncrementColumnTable>());
			}
		}

		[Test]
		public void TestFailedToCreateTableForAutoIncrementColumnWithString()
		{
			using(var connection = provider.GetOpenConnection())
			{
				Assert.Throws<SQLiteException>(
					() => connection.CreateTable<AutoIncrementColumnWithStringTable>());
			}
		}

		[Test]
		public void TestCreateMultiColumnPrimaryKeyTable()
		{
			using(var connection = provider.GetOpenConnection())
			{
				Assert.DoesNotThrow(
					() => connection.CreateTable<MultiColumnPrimaryKeyTable>());
			}
		}

		[Test]
		public void TestFailedToCreateInvalidMultiColumnPrimaryKeyTable()
		{
			using(var connection = provider.GetOpenConnection())
			{
				Assert.Throws<SQLiteException>(
					() => connection.CreateTable<InvalidMultiColumnPrimaryKeyTable>());
			}
		}

		[Test]
		public void TestCreateForeignKeyTable()
		{
			using(var connection = provider.GetOpenConnection())
			{
				Assert.DoesNotThrow(
					() =>
					{
						connection.CreateTable<OnlyPrimaryKeyTable>();
						connection.CreateTable<SpecifiedForeignKeyTable>();
					});
			}
		}

		[Test]
		public void TestCreateMultiColumnForeignKeyTable()
		{
			using(var connection = provider.GetOpenConnection())
			{
				Assert.DoesNotThrow(
					() =>
				{
					connection.CreateTable<MultiColumnPrimaryKeyTable>();
					connection.CreateTable<SpecifiedMultiColumnForeignKeyTable>();
				});
			}
		}

		[Test]
		public void TestCreateMultiColumnForeignKeysTable()
		{
			using(var connection = provider.GetOpenConnection())
			{
				Assert.DoesNotThrow(
					() =>
				{
					connection.CreateTable<MultiColumnPrimaryKeyTable>();
					connection.CreateTable<SpecifiedMultiColumnForeignKeysTable>();
				});
			}
		}

		[Test]
		public void TestCreateTableAsync()
		{
			using(var connection = provider.GetOpenConnection())
			{
				Assert.DoesNotThrow(
					() =>  connection.CreateTableAsync<OnlyPrimaryKeyTable>().Wait());
			}
		}
	}
}
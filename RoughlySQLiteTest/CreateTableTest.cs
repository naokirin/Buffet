using NUnit.Framework;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using RoughlySQLite;

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

	[TestFixture]
	public class CreateTableTest
	{
		string dbFileName;
		SqliteConnectionProvider provider;

		[TestFixtureSetUp]
		public void SetUpFixture()
		{
			dbFileName = DbTest.GetTempFileName();
		}

		[SetUp]
		public void SetUp()
		{
			provider = new SqliteConnectionProvider(dbFileName);
		}

		[TearDown]
		public void TearDown()
		{
			SqliteConnectionProvider.DeleteDBFile(dbFileName);
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
				Assert.Throws<SqliteException>(
					() => connection.CreateTable<AutoIncrementColumnWithStringTable>(),
					"SQLite error" + System.Environment.NewLine + "AUTOINCREMENT is only allowed on an INTEGER PRIMARY KEY");
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
				Assert.Throws<SqliteException>(
					() => connection.CreateTable<InvalidMultiColumnPrimaryKeyTable>(),
					"SQLite error" + System.Environment.NewLine + "table \"InvalidMultiColumnPrimaryKeyTable\" has more than one primary key");
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
	}
}
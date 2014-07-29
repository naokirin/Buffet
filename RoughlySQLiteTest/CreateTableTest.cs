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

		[Test]
		public void TestCreateCheckConstraintTable()
		{
			using(var connection = provider.GetOpenConnection())
			{
				Assert.DoesNotThrow(
					() => connection.CreateTable<CheckConstraintTable>());
			}
		}

		[Test]
		public void TestUniqueColumnTable()
		{
			using(var connection = provider.GetOpenConnection())
			{
				Assert.DoesNotThrow(
					() => connection.CreateTable<UniqueColumnTable>());
			}
		}

		[Test]
		public void TestMutliColumnUniqueColumnTable()
		{
			using(var connection = provider.GetOpenConnection())
			{
				Assert.DoesNotThrow(
					() => connection.CreateTable<MultiColumnUniqueTable>());
			}
		}
	}
}
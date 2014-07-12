using NUnit.Framework;
using RoughlySQLite;

#if NET45
using SQLiteConnection = System.Data.SQLite.SQLiteConnection;
using SQLiteException = System.Data.SQLite.SQLiteException;
#else
using SQLiteConnection = Mono.Data.Sqlite.SqliteConnection;
using SQLiteException = Mono.Data.Sqlite.SqliteException;
#endif

using System.Diagnostics;

namespace RoughlySQLiteTest
{
	public class InsertTest
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
		public void TestInsertOnlyPrimaryKeyTable()
		{
			using(var connection = provider.GetOpenConnection())
			{
				connection.CreateTable<OnlyPrimaryKeyTable>();
				Assert.DoesNotThrow(() => connection.Insert<OnlyPrimaryKeyTable>(new OnlyPrimaryKeyTable{ PKey = "" }));
			}
		}

		[Test]
		public void TestInsertAutoIncrementColumnTable()
		{
			using(var connection = provider.GetOpenConnection())
			{
				connection.CreateTable<AutoIncrementColumnTable>();
				Assert.DoesNotThrow(() => connection.Insert<AutoIncrementColumnTable>(new AutoIncrementColumnTable()));
			}
		}

		[Test]
		public void TestInsertAutoIncrementColumnWithOthersTable()
		{
			using(var connection = provider.GetOpenConnection())
			{
				connection.CreateTable<AutoIncrementColumnWithOthersTable>();
				Assert.DoesNotThrow(() => connection.Insert<AutoIncrementColumnWithOthersTable>(
					new AutoIncrementColumnWithOthersTable{ TestColumn = "" }));
			}
		}
	}
}


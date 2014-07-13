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

		[Test]
		public void TestInsertAsyncTable()
		{
			using(var connection = provider.GetOpenConnection())
			{
				connection.CreateTable<OnlyPrimaryKeyTable>();
				Assert.DoesNotThrow(() => connection.InsertAsync<OnlyPrimaryKeyTable>(new OnlyPrimaryKeyTable{ PKey = "" }).Wait());
			}
		}

		[Test]
		public void TestInsertRowCountTable()
		{
			using(var connection = provider.GetOpenConnection())
			{
				connection.CreateTable<OnlyPrimaryKeyTable>();
				Assert.That(connection.Insert<OnlyPrimaryKeyTable>(new OnlyPrimaryKeyTable{ PKey = "" }), Is.EqualTo(1));
			}
		}

		[Test]
		public void TestInsertDateTime()
		{
			using(var connection = provider.GetOpenConnection())
			{
				connection.CreateTable<DateTimeColumnTable>();
				Assert.DoesNotThrow(() => connection.Insert<DateTimeColumnTable>(new DateTimeColumnTable{ DateTime = new System.DateTime(2014, 7, 1) }));
			}
		}

		[Test]
		public void TestInsertSerializableData()
		{
			using(var connection = provider.GetOpenConnection())
			{
				connection.CreateTable<SerializedColumnTable>();
				Assert.DoesNotThrow(() => connection.Insert<SerializedColumnTable>(new SerializedColumnTable{ Data = new SerializableData() }));
			}
		}
			
		[Test]
		public void TestInsertBlob()
		{
			using(var connection = provider.GetOpenConnection())
			{
				connection.CreateTable<BlobColumnTable>();
				Assert.DoesNotThrow(() => connection.Insert<BlobColumnTable>(new BlobColumnTable{ Bytes = new byte[]{1, 1, 1} }));
			}
		}

		[Test]
		public void TestInsertEnum()
		{
			using(var connection = provider.GetOpenConnection())
			{
				connection.CreateTable<EnumColumnTable>();
				Assert.DoesNotThrow(() => connection.Insert<EnumColumnTable>(new EnumColumnTable {Enum = ColumnTestEnum.TypeA }));
			}
		}
	}
}


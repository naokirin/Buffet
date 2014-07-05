using NUnit.Framework;
using System;

class OnlyPrimaryKeyTable
{
	[PrimaryKey]
	public string PKey { get; set; }
}

class AutoIncrementColumnTable
{
	[PrimaryKey(IsAutoIncrement = true)]
	public int PKey { get; set;}
}

class AutoIncrementColumnWithStringTable
{
	[PrimaryKey(IsAutoIncrement = true)]
	public string PKey { get; set;}
}

[TestFixture]
public class CreateTableTest
{
	SqliteConnectionProvider provider;

	[SetUp]
	public void SetUp()
	{
		string dbFileName = DbTest.GetTempFileName();
		SqliteConnectionProvider.DeleteDBFile(dbFileName);
		provider = new SqliteConnectionProvider(dbFileName);
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
			Assert.Throws<NotAllowedTypeWithAutoIncrementException>(
				() => connection.CreateTable<AutoIncrementColumnWithStringTable>());
		}
	}
}

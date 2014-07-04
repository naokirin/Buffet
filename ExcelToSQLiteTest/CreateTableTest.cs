using NUnit.Framework;
using System;

class OnlyPrimaryKeyTable
{
	[PrimaryKey]
	public string PKey { get; set; }
}

[TestFixture()]
public class CreateTableTest
{
	[Test()]
	public void TestCreateOnlyPrimaryKeyTable()
	{
		string dbFileName = DbTest.GetTempFileName();
		SqliteConnectionProvider.DeleteDBFile(dbFileName);
		var db = new SqliteConnectionProvider(dbFileName);
		using(var connection = db.GetOpenConnection())
		{
			Assert.DoesNotThrow(() => connection.CreateTable<OnlyPrimaryKeyTable>());
		}
	}
}

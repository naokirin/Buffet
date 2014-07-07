using System;
#if NET45
using SQLiteConnection = System.Data.SQLite.SQLiteConnection;
using SQLiteException = System.Data.SQLite.SQLiteException;
#else
using SQLiteConnection = Mono.Data.Sqlite.SqliteConnection;
using SQLiteException = Mono.Data.Sqlite.SqliteException;
#endif

namespace RoughlySQLite
{
	public static class InsertExtension
	{
		public static void Insert<T>(this SQLiteConnection conn)
		{
			try
			{
				new Insert(typeof(T)).Exec(conn);
			}
			catch(SQLiteException e)
			{
				Console.WriteLine(e.Message);
				throw e;
			}
		}
	}

	class Insert
	{
		string sql = "";

		Type TableType { get; set; }

		string TableName { get; set; }

		public Insert(Type t)
		{
			TableType = t;
			sql = GetSqlCommand(t);
		}

		public void Exec(SQLiteConnection conn)
		{
			Console.WriteLine(sql + System.Environment.NewLine);
			using(var cmd = conn.CreateCommand())
			{
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
			}
		}

		string GetSqlCommand(Type t)
		{
			return "";
		}
	}
}


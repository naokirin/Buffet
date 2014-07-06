using System;
using Mono.Data.Sqlite;

namespace RoughlySQLite
{
	public static class InsertExtension
	{
		public static void Insert<T>(this SqliteConnection conn)
		{
			try
			{
				new Insert(typeof(T)).Exec(conn);
			}
			catch(SqliteException e)
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

		public void Exec(SqliteConnection conn)
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


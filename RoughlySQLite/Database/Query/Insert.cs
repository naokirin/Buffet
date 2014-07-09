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

	class Insert : NonQeuryCommand
	{
		public Insert(Type t) : base(t)
		{
		}

		override protected string GetSqlCommand(Type t)
		{
			return "";
		}
	}
}


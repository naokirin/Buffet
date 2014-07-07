using System;
using System.IO;

#if NET45
using SQLiteConnection = System.Data.SQLite.SQLiteConnection;
using SQLiteException = System.Data.SQLite.SQLiteException;
#else
using SQLiteConnection = Mono.Data.Sqlite.SqliteConnection;
using SQLiteException = Mono.Data.Sqlite.SqliteException;
#endif

namespace RoughlySQLite
{

	public sealed class SQLiteConnectionProvider : IDisposable
	{
		string dbFileName = "";
		SQLiteConnection conn = null;

		public SQLiteConnectionProvider(string dbFileName)
		{
			this.dbFileName = dbFileName;
		}

		public SQLiteConnection GetOpenConnection()
		{
			if (conn != null)
			{
				conn.Open();
				return conn;
			}

			// Create our connection
			bool exists = File.Exists(dbFileName);

			if (!exists) SQLiteConnection.CreateFile(dbFileName);

			conn = new SQLiteConnection("DbLinqProvider=SQLite;Data Source=" + dbFileName + ";Foreign Keys=true;");

			conn.Open();
			return conn;
		}

		public void Dispose()
		{
			conn.Close();
		}

		public static void DeleteDBFile(string dbFileName)
		{
			if (File.Exists(dbFileName)) File.Delete(dbFileName);
		}
	}
}

using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;
using System.Threading.Tasks;

namespace RoughlySQLite
{

	public sealed class SqliteConnectionProvider : IDisposable
	{
		private string dbFileName = "";
		private SqliteConnection conn = null;

		public SqliteConnectionProvider(string dbFileName)
		{
			this.dbFileName = dbFileName;
		}

		public SqliteConnection GetOpenConnection()
		{
			if (conn != null)
			{
				conn.Open();
				return conn;
			}

			// Create our connection
			bool exists = File.Exists(dbFileName);

			if (!exists) SqliteConnection.CreateFile(dbFileName);

			conn = new SqliteConnection("DbLinqProvider=Sqlite;Data Source=" + dbFileName + ";Foreign Keys=true;");

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

using System;
using System.Threading.Tasks;

#if NET45
using SQLiteConnection = System.Data.SQLite.SQLiteConnection;
using SQLiteException = System.Data.SQLite.SQLiteException;
using SQLiteCommand = System.Data.SQLite.SQLiteCommand;
#else
using SQLiteConnection = Mono.Data.Sqlite.SqliteConnection;
using SQLiteException = Mono.Data.Sqlite.SqliteException;
using SQLiteCommand = Mono.Data.Sqlite.SqliteCommand;
#endif

namespace RoughlySQLite
{
	class NonQeuryCommand
	{
		protected Type TableType { get; set; }

		public NonQeuryCommand(Type t)
		{
			TableType = t;
		}

		public void Exec(SQLiteConnection conn)
		{
			Console.WriteLine(GetQueryString() + System.Environment.NewLine);
			using(var cmd = conn.CreateCommand())
			{
				SetQuery(cmd);
				cmd.ExecuteNonQuery();
			}
		}

		public async Task ExecAsync(SQLiteConnection conn)
		{
			Console.WriteLine(GetQueryString() + System.Environment.NewLine);
			using(var cmd = conn.CreateCommand())
			{
				SetQuery(cmd);
				await cmd.ExecuteNonQueryAsync();
			}
		}

		protected virtual void SetQuery(SQLiteCommand command)
		{
		}

		protected virtual string GetQueryString()
		{
			return "";
		}
	}
}


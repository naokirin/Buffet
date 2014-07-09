using System;
using System.Threading.Tasks;

#if NET45
using SQLiteConnection = System.Data.SQLite.SQLiteConnection;
using SQLiteException = System.Data.SQLite.SQLiteException;
#else
using SQLiteConnection = Mono.Data.Sqlite.SqliteConnection;
using SQLiteException = Mono.Data.Sqlite.SqliteException;
#endif

namespace RoughlySQLite
{
	public class NonQeuryCommand
	{
		protected string sql = "";

		protected Type TableType { get; set; }

		protected string TableName { get; set; }

		public NonQeuryCommand(Type t)
		{
			Initialize(t);
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

		public async Task ExecAsync(SQLiteConnection conn)
		{
			Console.WriteLine(sql + System.Environment.NewLine);
			using(var cmd = conn.CreateCommand())
			{
				cmd.CommandText = sql;
				await cmd.ExecuteNonQueryAsync();
			}
		}

		void Initialize(Type t)
		{
			TableType = t;
			sql = GetSqlCommand(t);
		}

		protected virtual string GetSqlCommand(Type t)
		{
			return "";
		}
	}
}


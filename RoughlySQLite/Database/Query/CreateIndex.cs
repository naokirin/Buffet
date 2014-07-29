using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#if NET45
using SQLiteConnection = System.Data.SQLite.SQLiteConnection;
using SQLiteException = System.Data.SQLite.SQLiteException;
using SQLiteParameter = System.Data.SQLite.SQLiteParameter;
using SQLiteCommand = System.Data.SQLite.SQLiteCommand;

#else
using SQLiteConnection = Mono.Data.Sqlite.SqliteConnection;
using SQLiteException = Mono.Data.Sqlite.SqliteException;
using SQLiteParameter = Mono.Data.Sqlite.SqliteParameter;
using SQLiteCommand = Mono.Data.Sqlite.SqliteCommand;
#endif

namespace RoughlySQLite
{
	public static class CreateIndexExtension
	{
		public static void CreateIndex<T>(this SQLiteConnection conn)
		{
			try
			{
				new CreateIndex(typeof(T)).Exec(conn);
			}
			catch(SQLiteException e)
			{
				Console.WriteLine(e.Message);
				throw e;
			}
		}

		public static async Task CreateIndexAsync<T>(this SQLiteConnection conn)
		{
			try
			{
				await new CreateIndex(typeof(T)).ExecAsync(conn);
			}
			catch(SQLiteException e)
			{
				Console.WriteLine(e.Message);
				throw e;
			}
		}
	}

	class CreateIndex : NonQeuryCommand
	{
		string sql = "";


		public CreateIndex(Type t) : base(t)
		{
			sql = GetSqlCommand(t);
		}

		protected override void SetQuery(SQLiteCommand command)
		{
			command.CommandText = sql;
		}

		protected override string GetQueryString()
		{
			return sql;
		}

		string GetSqlCommand(Type t)
		{
			var table = Table.GetTable(t);

			return table.Indexes.Aggregate("",
				(query, index) => query
				+ string.Format("CREATE {0}INDEX {1} ON {2}({3}); ",
					index.Unique ? "Unique " : "",
					index.Name,
					table.TableName,
					string.Join(", ", index.Columns)
				)); 
		}
	}
}


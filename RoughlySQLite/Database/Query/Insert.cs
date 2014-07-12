using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
#if NET45
using SQLiteConnection = System.Data.SQLite.SQLiteConnection;
using SQLiteException = System.Data.SQLite.SQLiteException;
using SQLiteParameter = System.Data.SQLite.SQLiteParameter;
#else
using SQLiteConnection = Mono.Data.Sqlite.SqliteConnection;
using SQLiteException = Mono.Data.Sqlite.SqliteException;
using SQLiteParameter = Mono.Data.Sqlite.SqliteParameter;
#endif

namespace RoughlySQLite
{
	public static class InsertExtension
	{
		public static void Insert<T>(this SQLiteConnection conn, T value)
		{
			try
			{
				new Insert<T>(value).Exec(conn);
			}
			catch(SQLiteException e)
			{
				Console.WriteLine(e.Message);
				throw e;
			}
		}

		public static async Task InsertAsync<T>(this SQLiteConnection conn, T value)
		{
			try
			{
				await new Insert<T>(value).ExecAsync(conn);
			}
			catch(SQLiteException e)
			{
				Console.WriteLine(e.Message);
				throw e;
			}
		}
	}

	class Insert<T> : NonQeuryCommand
	{
		ParameterizedQuery Query { get; set; }

		public Insert(T value) : base(typeof(T))
		{
			Query = GetQuery(typeof(T), value);
		}

		public void Exec(SQLiteConnection conn)
		{
			Console.WriteLine(Query.QueryString + System.Environment.NewLine);
			using(var cmd = conn.CreateCommand())
			{
				cmd.CommandText = Query.QueryString;
				cmd.CommandType = CommandType.Text;
				Query.Parameters.ToList()
					.ForEach(x => cmd.Parameters.Add(new SQLiteParameter(x.Value.Parameter, x.Value.ReplaceString)));
				cmd.ExecuteNonQuery();
			}
		}

		public async Task ExecAsync(SQLiteConnection conn)
		{
			Console.WriteLine(Query.QueryString + System.Environment.NewLine);
			using(var cmd = conn.CreateCommand())
			{
				cmd.CommandText = Query.QueryString;
				cmd.CommandType = CommandType.Text;
				Query.Parameters.ToList()
					.ForEach(x => cmd.Parameters.Add(new SQLiteParameter(x.Value.Parameter, x.Value.ReplaceString)));
				await cmd.ExecuteNonQueryAsync();
			}
		}

		ParameterizedQuery GetQuery(Type t, T value)
		{
			var table = Table.GetTable(t);

			var columns = table.Columns;
			var columnsString =  String.Join(", ", columns.Select(x => x.Name).ToList());
			var parameters = String.Join(", ", columns.Select(x => x.SpecifiedPrimaryKey.IsAutoIncrement ? "NULL" : "@" + x.Name).ToList());
			var values = new List<ParameterizedString>();
			columns.ForEach(x => {
				if (x.SpecifiedPrimaryKey.IsAutoIncrement) return;
				values.Add(new ParameterizedString {
					Parameter= x.Name,
					ReplaceString=x.Getter.GetValue(value).ToSQLiteValue(x.ColumnType)
					});
				});

			return new ParameterizedQuery(
				String.Format("INSERT INTO {0}({1})" + System.Environment.NewLine + "VALUES ({2})", table.TableName, columnsString, parameters),
				values);
		}
	}
}


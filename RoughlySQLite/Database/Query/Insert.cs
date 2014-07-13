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
using SQLiteCommand = System.Data.SQLite.SQLiteCommand;
#else
using SQLiteConnection = Mono.Data.Sqlite.SqliteConnection;
using SQLiteException = Mono.Data.Sqlite.SqliteException;
using SQLiteParameter = Mono.Data.Sqlite.SqliteParameter;
using SQLiteCommand = Mono.Data.Sqlite.SqliteCommand;
#endif

namespace RoughlySQLite
{
	public static class InsertExtension
	{
		public static int Insert<T>(this SQLiteConnection conn, T value)
		{
			try
			{
				return new Insert<T>(value).Exec(conn);
			}
			catch(SQLiteException e)
			{
				Console.WriteLine(e.Message);
				throw e;
			}
		}

		public static async Task<int> InsertAsync<T>(this SQLiteConnection conn, T value)
		{
			try
			{
				return await new Insert<T>(value).ExecAsync(conn);
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

		protected override void SetQuery(SQLiteCommand command)
		{
			command.CommandText = Query.QueryString;
			command.CommandType = CommandType.Text;
			Query.Parameters.ToList()
				.ForEach(x => {
					var parameter = new SQLiteParameter(x.Key, x.Value.Replacing);
					if (x.Value.Type == SQLiteType.Blob) parameter.DbType = DbType.Binary;
					command.Parameters.Add(parameter);
				});
		}

		protected override string GetQueryString()
		{
			return Query.QueryString;
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
					Replacing=x.Getter.GetValue(value).ToSQLiteValue(x.ColumnType),
					Type = x.ColumnType.ToSQLiteType()
					});
				});

			return new ParameterizedQuery(
				String.Format("INSERT INTO {0}({1})" + System.Environment.NewLine + "VALUES ({2})", table.TableName, columnsString, parameters),
				values);
		}
	}
}


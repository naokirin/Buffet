using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
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
	public static class CreateTableExtension
	{
		public static void CreateTable<T>(this SQLiteConnection conn)
		{
			try
			{
				new CreateTable(typeof(T)).Exec(conn);
			}
			catch(SQLiteException e)
			{
				Console.WriteLine(e.Message);
				throw e;
			}
		}

		public static async Task CreateTableAsync<T>(this SQLiteConnection conn)
		{
			try
			{
				await new CreateTable(typeof(T)).ExecAsync(conn);
			}
			catch(SQLiteException e)
			{
				Console.WriteLine(e.Message);
				throw e;
			}
		}
	}

	class CreateTable : NonQeuryCommand
	{
		string sql = "";


		public CreateTable(Type t) : base(t)
		{
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

		public async Task ExecAsync(SQLiteConnection conn)
		{
			Console.WriteLine(sql + System.Environment.NewLine);
			using(var cmd = conn.CreateCommand())
			{
				cmd.CommandText = sql;
				await cmd.ExecuteNonQueryAsync();
			}
		}

		string GetSqlCommand(Type t)
		{
			return GetTableData(t);
		}

		string GetTableData(Type t)
		{
			var table = Table.GetTable(t);
			return String.Format("CREATE TABLE IF NOT EXISTS {0}{1}{2};",
				table.TableName, System.Environment.NewLine, GetColumnsQueryString(table));
		}

		string GetColumnsQueryString(Table table)
		{
			string columnQuery = "( ";
			bool isFirst = true;
			table.Columns.ForEach(column =>
			{
				if (isFirst) isFirst = false;
				else columnQuery += ", ";
				columnQuery +=
			column.Name + " "
				+ column.ColumnType.ToSQLiteTypeString()
				+ (column.IsNullable ? "" : " NOT NULL")
				+ (column.SpecifiedPrimaryKey.IsPrimaryKey ? " PRIMARY KEY" : "")
				+ (column.SpecifiedPrimaryKey.IsAutoIncrement ? " AUTOINCREMENT" : "")
				+ System.Environment.NewLine;
			});

			columnQuery += GetPrimaryKeyQueryString(table);
			columnQuery += GetForeignKeyQueryString(table);

			columnQuery += ")";

			return columnQuery;
		}

		string GetPrimaryKeyQueryString(Table table)
		{
			var columnQuery = "";
			if (table.SpecifiedMultiColumnPrimaryKey != null && table.SpecifiedMultiColumnPrimaryKey.Columns.Any())
			{
				bool isFirst = true;
				table.SpecifiedMultiColumnPrimaryKey.Columns.ForEach(column =>
				{
					if (isFirst)
					{
						columnQuery += ", PRIMARY KEY (";
						isFirst = false;
					}
					else
					{
						columnQuery += ", ";
					}
					columnQuery += column;
				});
				columnQuery += ")" + System.Environment.NewLine;
			}

			return columnQuery;
		}

		string GetForeignKeyQueryString(Table table)
		{
			var columnQuery = "";
			var foreignKeyColumns = table.Columns.Where(column => column.SpecifiedForeignKey != null).ToList();
			if (foreignKeyColumns.Any())
			{
				foreignKeyColumns.ForEach(column =>
				{
					columnQuery += ", FOREIGN KEY (";
					columnQuery += column.Name;
					columnQuery += ") ";

					columnQuery += "REFERENCES ";
					columnQuery += column.SpecifiedForeignKey.ForeignTable;
					columnQuery += "(" + column.SpecifiedForeignKey.ReferencedColumn + ")";

					columnQuery += " ON UPDATE " + column.SpecifiedForeignKey.OnUpdateAction.GetQueryString();
					columnQuery += " ON DELETE " + column.SpecifiedForeignKey.OnDeleteAction.GetQueryString();
					columnQuery += System.Environment.NewLine;
				});
			}

			if (table.SpecifiedMultiColumnForeignKeys != null && table.SpecifiedMultiColumnForeignKeys.Any())
			{
				table.SpecifiedMultiColumnForeignKeys.ForEach(key =>
				{
					columnQuery += ", FOREIGN KEY (";
					columnQuery += String.Join(", ", key.Columns);
					columnQuery += ") ";

					columnQuery += "REFERENCES ";
					columnQuery += key.ForeignTable;
					columnQuery += "(" + String.Join(", ", key.ReferencedColumns) + ")";

					columnQuery += " ON UPDATE " + key.OnUpdateAction.GetQueryString();
					columnQuery += " ON DELETE " + key.OnDeleteAction.GetQueryString();
					columnQuery += System.Environment.NewLine;
				}
				);
			}

			return columnQuery;
		}
	}
}

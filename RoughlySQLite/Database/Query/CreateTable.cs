﻿using System;
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
		public CreateTable(Type t) : base(t)
		{
		}

		protected override string GetSqlCommand(Type t)
		{
			return GetTableData(t);
		}

		string GetTableData(Type t)
		{
			var tableAttr = (TableAttribute)t.GetAttribute(typeof(TableAttribute));

			TableName = tableAttr != null ? tableAttr.Name : TableType.Name;

			var multiColumnPrimaryKeyAttr = (MultiColumnPrimaryKeyAttribute)t.GetAttribute(typeof(MultiColumnPrimaryKeyAttribute));
			var multiColumnPrimaryKey = new SpecifiedMultiColumnPrimaryKey();
			multiColumnPrimaryKey.Columns = multiColumnPrimaryKeyAttr != null ? multiColumnPrimaryKeyAttr.Columns : new List<string>();

			var multiColumnForeignKeyAttrs = (MultiColumnForeignKeyAttribute[])t.GetAttributes(typeof(MultiColumnForeignKeyAttribute));
			var multiColumnForeignKeys = new List<SpecifiedMultiColumnForeignKey>();
			if (multiColumnForeignKeyAttrs != null)
			{
				multiColumnForeignKeyAttrs.ToList().ForEach(x =>
				{
					var key = new SpecifiedMultiColumnForeignKey();
					var referencedTableAttr = (TableAttribute)x.ReferencedTableType.GetAttribute(typeof(TableAttribute));
					key.ForeignTable = referencedTableAttr != null ? referencedTableAttr.Name : x.ReferencedTableType.Name;
					key.ReferencedColumns = x.ReferencedColumns;
					key.Columns = x.Columns;
					key.OnDeleteAction = x.OnDeleteAction;
					key.OnUpdateAction = x.OnUpdateAction;
					multiColumnForeignKeys.Add(key);
				});
			}

			var properties = from property in t.GetRuntimeProperties()
			                where (property.GetMethod != null && property.GetMethod.IsPublic)
			                    || (property.SetMethod != null && property.SetMethod.IsPublic)
			                    || (property.GetMethod != null && property.GetMethod.IsStatic)
			                    || (property.SetMethod != null && property.SetMethod.IsStatic)
			                select property;

			var columns = new List<Column>();
			foreach(var property in properties)
			{
				var ignore = property.GetCustomAttributes(typeof(IgnoreAttribute), true).Any();

				if (property.CanWrite && !ignore)
				{
					columns.Add(new Column(property));
				}
			}

			var table = new Table(TableName, columns, multiColumnPrimaryKey, multiColumnForeignKeys);

			return String.Format("CREATE TABLE IF NOT EXISTS {0}{1}{2};",
				TableName, System.Environment.NewLine, GetColumnsQueryString(table));
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

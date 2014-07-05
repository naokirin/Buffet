using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Reflection;
using System.Linq;


public static class CreateTableExtension
{
	public static void CreateTable<T>(this SqliteConnection conn)
	{
		new CreateTable(typeof(T)).Exec(conn);
	}
}

public class CreateTable
{
	private string sql = "";
	private Type TableType { get; set; }
	private string TableName { get; set; }

	public CreateTable(Type t)
	{
		TableType = t;
		sql = GetSqlCommand(t);
		Console.WriteLine(sql + System.Environment.NewLine);
	}

	public void Exec(SqliteConnection conn)
	{
		using (var cmd = conn.CreateCommand ())
		{
			cmd.CommandText = sql;
			cmd.ExecuteNonQuery ();
		}
	}

	private string GetSqlCommand(Type t)
	{
		return GetTableData(t);
	}

	private string GetTableData(Type t)
	{
		var tableAttr = (TableAttribute)Attribute.GetCustomAttribute(
			t, typeof(TableAttribute), true);

		TableName = tableAttr != null ? tableAttr.Name : TableType.Name;

		var properties = from property in t.GetRuntimeProperties()
				where (property.GetMethod != null && property.GetMethod.IsPublic)
					|| (property.SetMethod != null && property.SetMethod.IsPublic)
					|| (property.GetMethod != null && property.GetMethod.IsStatic)
					|| (property.SetMethod != null && property.SetMethod.IsStatic)
			select property;

		var columns = new List<Column>();
		foreach(var property in properties) {
			var ignore = property.GetCustomAttributes(typeof(IgnoreAttribute), true).Any();

			if (property.CanWrite && !ignore) {
				columns.Add(new Column(TableName, property));
			}
		}

		return String.Format("CREATE TABLE IF NOT EXISTS {0}{1}{2};",
			TableName, System.Environment.NewLine, GetColumnsQueryString(columns));
	}

	private string GetColumnsQueryString(List<Column> columns)
	{
		string columnQuery = "( ";
		bool isFirst = true;
		columns.ForEach(column => {
			if (isFirst) isFirst = false;
			else columnQuery += ", ";
			columnQuery +=
			column.Name + " "
				+ column.ColumnType.ToSQLiteTypeString()
				+ (column.IsNullable ? "" : " NOT NULL")
				+ (column.SpecifiedPrimaryKey.IsPrimaryKey
					&& columns.Where(x => x.SpecifiedPrimaryKey.IsPrimaryKey).Count() == 1 ? " PRIMARY KEY" : "")
				+ (column.SpecifiedPrimaryKey.IsAutoIncrement ? " AUTOINCREMENT" : "")
				+ System.Environment.NewLine;
		});

		columnQuery += GetPrimaryKeyQueryString(columns);
		columnQuery += GetForeignKeyQueryString(columns);

		columnQuery += ")";

		return columnQuery;
	}

	private string GetPrimaryKeyQueryString(List<Column> columns)
	{
		var columnQuery = "";
		var primaryKeyColumns = columns.Where(column => column.SpecifiedPrimaryKey.IsPrimaryKey).ToList();
		if (primaryKeyColumns.Count <= 1) return "";
		if (primaryKeyColumns.Any())
		{
			bool isFirst = true;
			primaryKeyColumns.ForEach(column => {
				if (isFirst)
				{
					columnQuery += ", PRIMARY KEY (";
					isFirst = false;
				}
				else
				{
					columnQuery += ", ";
				}
				columnQuery += column.Name;
			});
			columnQuery += ")" + System.Environment.NewLine;
		}

		return columnQuery;
	}

	private string GetForeignKeyQueryString(List<Column> columns)
	{
		var columnQuery = "";
		var foreignKeyColumns = columns.Where(column => column.SpecifiedForeignKey != null).ToList();
		if (foreignKeyColumns.Any())
		{
			foreignKeyColumns.ForEach(column => {
				columnQuery += ", FOREIGN KEY (";
				columnQuery += column.Name;
				columnQuery += ") ";

				columnQuery += "REFERENCES ";
				columnQuery += column.SpecifiedForeignKey.ForeignTable;
				columnQuery += "(" + column.SpecifiedForeignKey.ForeignColumn + ")";

				columnQuery += " ON UPDATE " + column.SpecifiedForeignKey.OnUpdateAction.GetQueryString();
				columnQuery += " ON DELETE " + column.SpecifiedForeignKey.OnDeleteAction.GetQueryString();
				columnQuery += System.Environment.NewLine;
			});
		}

		return columnQuery;
	}
}


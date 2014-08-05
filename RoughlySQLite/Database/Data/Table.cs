using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RoughlySQLite
{
	class Table
	{
		public string TableName { get; private set; }

		public List<Column> Columns { get; private set; }

		public SpecifiedMultiColumnPrimaryKey SpecifiedMultiColumnPrimaryKey { get; private set; }

		public List<CheckConstraint> CheckConstraints { get; set; }

		public List<SpecifiedMultiColumnForeignKey> SpecifiedMultiColumnForeignKeys { get; private set; }

		public List<Index> Indexes { get; private set; }

		public Table(string tableName, List<Column> columns, SpecifiedMultiColumnPrimaryKey multiColumnPrimaryKey,
			List<SpecifiedMultiColumnForeignKey> multiColumnForeignKeys, List<CheckConstraint> checks, List<Index> indexes)
		{
			TableName = tableName;
			Columns = columns;
			SpecifiedMultiColumnPrimaryKey = multiColumnPrimaryKey;
			SpecifiedMultiColumnForeignKeys = multiColumnForeignKeys;
			CheckConstraints = checks;
			Indexes = indexes;
		}

		public static Table GetTable(Type t)
		{
			var tableAttr = (TableAttribute)t.GetAttribute(typeof(TableAttribute));

			var tableName = tableAttr != null ? tableAttr.Name : t.Name;

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

			var checkAttrs = (CheckAttribute[])t.GetAttributes(typeof(CheckAttribute));
			var checks = new List<CheckConstraint>();
			if (checkAttrs != null)
			{
				checkAttrs.ToList().ForEach(x =>
				{
					var check = new CheckConstraint();
					check.Condition = x.Condition;
					checks.Add(check);
				});
			}

			var indexAttrs = (MultiColumnIndexAttribute[])t.GetAttributes(typeof(MultiColumnIndexAttribute));
			var indexes = new List<Index>();
			if (indexAttrs != null)
			{
				indexAttrs.ToList().ForEach(x =>
				{
					var index = new Index(x.Name, x.Columns.ToList(), x.Unique);
					indexes.Add(index);
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
				var ignore = property.GetCustomAttributes(typeof(IgnoredColumnAttribute), true).Any();

				if (property.CanWrite && !ignore)
				{
					columns.Add(new Column(property));
				}
			}

			return new Table(tableName, columns, multiColumnPrimaryKey, multiColumnForeignKeys, checks, indexes);
		}
	}
}

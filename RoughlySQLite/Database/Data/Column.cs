using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace RoughlySQLite
{
	class Column
	{
		public string Name { get; private set; }

		public Type ColumnType { get; private set; }

		public bool IsNullable { get; private set; }

		public SpecifiedPrimaryKey SpecifiedPrimaryKey { get; private set; }

		public SpecifiedForeignKey SpecifiedForeignKey { get; private set; }

		public Column(PropertyInfo prop)
		{
			var columnAttr = (ColumnAttribute)prop.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault();
			Name = columnAttr == null ? prop.Name : columnAttr.Name;

			ColumnType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

			var notNullAttr = (NotNullAttribute)prop.GetCustomAttributes(typeof(NotNullAttribute), true).FirstOrDefault();
			IsNullable = notNullAttr == null;

			SpecifiedPrimaryKey = new SpecifiedPrimaryKey();
			var primaryKeyAttr = (PrimaryKeyAttribute)prop.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).FirstOrDefault();
			SpecifiedPrimaryKey.IsPrimaryKey = primaryKeyAttr != null;
			if (SpecifiedPrimaryKey.IsPrimaryKey)
			{
				SpecifiedPrimaryKey.IsAutoIncrement = primaryKeyAttr.IsAutoIncrement;
			}
			else
			{
				SpecifiedPrimaryKey.IsAutoIncrement = false;
			}

			var foreignKeyAttr = (ForeignKeyAttribute)prop.GetCustomAttributes(typeof(ForeignKeyAttribute), true).FirstOrDefault();
			if (foreignKeyAttr != null)
			{
				SpecifiedForeignKey = new SpecifiedForeignKey();
				var tableInfo = (TableAttribute)foreignKeyAttr.ReferencedTableType.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault();
				SpecifiedForeignKey.ForeignTable = tableInfo != null ? tableInfo.Name : foreignKeyAttr.ReferencedTableType.Name;
				SpecifiedForeignKey.ReferencedColumn = foreignKeyAttr.Column;
				SpecifiedForeignKey.OnDeleteAction = foreignKeyAttr.OnDeleteAction;
				SpecifiedForeignKey.OnUpdateAction = foreignKeyAttr.OnUpdateAction;
			}
		}
	}
}

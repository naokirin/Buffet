using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace RoughlySQLite
{
	class Column
	{
		public IGetter Getter { get; private set; }

		public string Name { get; private set; }

		public Type ColumnType { get; private set; }

		public bool IsNullable { get; private set; }

		public SpecifiedPrimaryKey SpecifiedPrimaryKey { get; private set; }

		public SpecifiedForeignKey SpecifiedForeignKey { get; private set; }

		public bool Unique { get; private set; }

		public Column(PropertyInfo prop)
		{
			Getter = prop.ToGetter();

			var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
			Name = columnAttr == null ? prop.Name : columnAttr.Name;

			ColumnType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

			var notNullAttr = prop.GetCustomAttribute<NotNullAttribute>();
			IsNullable = notNullAttr == null;

			SpecifiedPrimaryKey = new SpecifiedPrimaryKey();
			var primaryKeyAttr = prop.GetCustomAttribute<PrimaryKeyAttribute>();
			SpecifiedPrimaryKey.IsPrimaryKey = primaryKeyAttr != null;
			if (SpecifiedPrimaryKey.IsPrimaryKey)
			{
				SpecifiedPrimaryKey.IsAutoIncrement = primaryKeyAttr.IsAutoIncrement;
			}
			else
			{
				SpecifiedPrimaryKey.IsAutoIncrement = false;
			}

			var foreignKeyAttr = prop.GetCustomAttribute<ForeignKeyAttribute>();
			if (foreignKeyAttr != null)
			{
				SpecifiedForeignKey = new SpecifiedForeignKey();
				var tableInfo = (TableAttribute)foreignKeyAttr.ReferencedTableType.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault();
				SpecifiedForeignKey.ForeignTable = tableInfo != null ? tableInfo.Name : foreignKeyAttr.ReferencedTableType.Name;
				SpecifiedForeignKey.ReferencedColumn = foreignKeyAttr.Column;
				SpecifiedForeignKey.OnDeleteAction = foreignKeyAttr.OnDeleteAction;
				SpecifiedForeignKey.OnUpdateAction = foreignKeyAttr.OnUpdateAction;
			}

			var uniqueAttr = prop.GetCustomAttribute<UniqueAttribute>();
			Unique = uniqueAttr != null;
		}
	}
}

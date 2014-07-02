﻿using System;
using System.Reflection;
using System.Linq;


public class Column
{
	public string Name { get; private set; }
	public Type ColumnType { get; private set; }
	public bool IsNullable { get; private set; }
	public bool IsAutoIncrement { get; private set; }
	public bool IsPrimaryKey { get; private set; }
	public SpecifiedForeignKey SpecifiedForeignKey { get; private set; }

	public Column(PropertyInfo prop)
	{
		var columnAttr = (ColumnAttribute)prop.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault();
		Name = columnAttr == null ? prop.Name : columnAttr.Name;

		ColumnType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

		var notNullAttr = (NotNullAttribute)prop.GetCustomAttributes(typeof(NotNullAttribute), true).FirstOrDefault();
		IsNullable = notNullAttr == null;

		var primaryKeyAttr = (PrimaryKeyAttribute)prop.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).FirstOrDefault();
		IsPrimaryKey = primaryKeyAttr != null;
		if(IsPrimaryKey)
		{
			IsAutoIncrement = primaryKeyAttr.IsAutoIncrement;
		}
		else
		{
			IsAutoIncrement = false;
		}

		var foreignKeyAttr = (ForeignKeyAttribute)prop.GetCustomAttributes(typeof(ForeignKeyAttribute), true).FirstOrDefault();
		if (foreignKeyAttr != null)
		{
			SpecifiedForeignKey = new SpecifiedForeignKey();
			var tableInfo = (TableAttribute)foreignKeyAttr.ForeignType.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault();
			SpecifiedForeignKey.ForeignTable = tableInfo != null ? tableInfo.Name : foreignKeyAttr.ForeignType.Name;
			SpecifiedForeignKey.ForeignColumn = foreignKeyAttr.ForeignColumn;
			SpecifiedForeignKey.OnDeleteAction = foreignKeyAttr.OnDeleteAction;
			SpecifiedForeignKey.OnUpdateAction = foreignKeyAttr.OnUpdateAction;
		}
	}
}

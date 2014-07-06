using System;
using System.Collections.Generic;

public enum OnDeleteAction
{
	Cascade,
	SetDefault,
	SetNull,
	Restrict,
	NoAction
}

public enum OnUpdateAction
{
	Cascade,
	SetDefault,
	SetNull,
	Restrict,
	NoAction
}

[AttributeUsage(AttributeTargets.Property)]
public class ForeignKeyAttribute : Attribute
{
	public Type ReferencedTableType { get; set; }
	public string Column { get; set; }

	private OnDeleteAction onDeleteAction = OnDeleteAction.NoAction;
	public OnDeleteAction OnDeleteAction {
		get { return onDeleteAction; }
		set { onDeleteAction = value; }
	}

	private OnUpdateAction onUpdateAction = OnUpdateAction.NoAction;
	public OnUpdateAction OnUpdateAction {
		get { return onUpdateAction; }
		set { onUpdateAction = value; }
	}

	public ForeignKeyAttribute(Type foreignType, string foreignColumn)
	{
		ReferencedTableType = foreignType;
		Column = foreignColumn;
	}
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class MultiColumnForeignKeyAttribute : Attribute
{
	public List<string> Columns { get; set; }
	public List<string> ReferencedColumns { get; set; }
	public Type ReferencedTableType { get; set; }

	private OnDeleteAction onDeleteAction = OnDeleteAction.NoAction;
	public OnDeleteAction OnDeleteAction {
		get { return onDeleteAction; }
		set { onDeleteAction = value; }
	}

	private OnUpdateAction onUpdateAction = OnUpdateAction.NoAction;
	public OnUpdateAction OnUpdateAction {
		get { return onUpdateAction; }
		set { onUpdateAction = value; }
	}

	public MultiColumnForeignKeyAttribute()
	{
	}

	public MultiColumnForeignKeyAttribute(string[] columns, Type referencedTableType, string[] referencedColumns)
	{
		Columns = new List<string>(columns);
		ReferencedTableType = referencedTableType;
		ReferencedColumns = new List<string>(referencedColumns);
	}
}
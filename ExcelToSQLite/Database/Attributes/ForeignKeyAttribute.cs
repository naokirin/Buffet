using System;

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
	public Type ForeignType { get; set; }
	public string ForeignColumn { get; set; }

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
		ForeignType = foreignType;
		ForeignColumn = foreignColumn;
	}
}
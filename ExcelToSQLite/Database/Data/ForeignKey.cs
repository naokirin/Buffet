using System;
using System.Collections.Generic;

public static class ForeignKeyUtil
{
	static Dictionary<OnDeleteAction, string> onDeleteActions = new Dictionary<OnDeleteAction, string> {
		{ OnDeleteAction.Cascade, "CASCADE" },
		{ OnDeleteAction.NoAction, "NO ACTION" },
		{ OnDeleteAction.Restrict, "RESTRICT" },
		{ OnDeleteAction.SetDefault, "SET DEFAULT" },
		{ OnDeleteAction.SetNull, "SET NULL" },
	};
	static Dictionary<OnUpdateAction, string> onUpdateActions = new Dictionary<OnUpdateAction, string> {
		{ OnUpdateAction.Cascade, "CASCADE" },
		{ OnUpdateAction.NoAction, "NO ACTION" },
		{ OnUpdateAction.Restrict, "RESTRICT" },
		{ OnUpdateAction.SetDefault, "SET DEFAULT" },
		{ OnUpdateAction.SetNull, "SET NULL" },
	};
	public static string GetQueryString(this OnDeleteAction onDeleteAction)
	{
		string result = "";
		onDeleteActions.TryGetValue(onDeleteAction, out result);
		return result;
	}
	public static string GetQueryString(this OnUpdateAction onUpdateAction)
	{
		string result = "";
		onUpdateActions.TryGetValue(onUpdateAction, out result);
		return result;
	}
}

public class SpecifiedForeignKey
{
	public string ForeignTable { get; set; }
	public string ReferencedColumn { get; set; }
	public OnDeleteAction OnDeleteAction { get; set; }
	public OnUpdateAction OnUpdateAction { get; set; }
}

public class SpecifiedMultiColumnForeignKey
{
	public string ForeignTable { get; set; }
	public List<string> Columns { get; set; }
	public List<string> ReferencedColumns { get; set; }
	public OnDeleteAction OnDeleteAction { get; set; }
	public OnUpdateAction OnUpdateAction { get; set; }
}
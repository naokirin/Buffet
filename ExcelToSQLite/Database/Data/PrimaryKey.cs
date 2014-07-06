using System;
using System.Collections.Generic;

public class SpecifiedPrimaryKey
{
	public bool IsAutoIncrement { get; set; }
	public bool IsPrimaryKey { get; set; }
}

public class SpecifiedMultiColumnPrimaryKey
{
	public List<string> Columns { get; set; }
}

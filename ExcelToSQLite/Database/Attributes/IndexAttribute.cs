using System;
using System.Collections.Generic;

[AttributeUsage (AttributeTargets.Class, AllowMultiple = true)]
public class IndexAttribute : Attribute
{
	public string Name { get; set; }
	public int Order { get; set; }
	public virtual bool Unique { get; set; }
	public List<string> Columns { get; set; }

	public IndexAttribute()
	{
	}

	public IndexAttribute(string name, int order, List<string> columns)
	{
		Name = name;
		Order = order;
		Columns = columns;
	}
}

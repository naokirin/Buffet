using System;

[AttributeUsage(AttributeTargets.Property)]
public class PrimaryKeyAttribute : Attribute
{
	private bool isAutoIncrement = false;
	public bool IsAutoIncrement
	{
		get { return isAutoIncrement; }
		set { isAutoIncrement = value; }
	}
}

using System;

namespace RoughlySQLite
{

	[AttributeUsage(AttributeTargets.Property)]
	public class IgnoredColumnAttribute : Attribute
	{
	}
}

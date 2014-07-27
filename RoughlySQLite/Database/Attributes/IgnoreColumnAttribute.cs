using System;

namespace RoughlySQLite
{

	[AttributeUsage(AttributeTargets.Property)]
	public class IgnoreColumnAttribute : Attribute
	{
	}
}

using System;

namespace RoughlySQLite
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class CheckAttribute : Attribute
	{
		public string Condition { get; set; }

		public CheckAttribute(string condition)
		{
			Condition = condition;
		}
	}
}

using System;
using System.Collections.Generic;

namespace RoughlySQLite
{

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

	[AttributeUsage(AttributeTargets.Class)]
	public class MultiColumnPrimaryKeyAttribute : Attribute
	{
		public List<string> Columns;

		public MultiColumnPrimaryKeyAttribute()
		{
		}

		public MultiColumnPrimaryKeyAttribute(string[] columns)
		{
			Columns = new List<string>(columns);
		}
	}
}

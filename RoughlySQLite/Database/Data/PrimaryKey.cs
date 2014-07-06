using System;
using System.Collections.Generic;

namespace RoughlySQLite
{

	public class SpecifiedPrimaryKey
	{
		public bool IsAutoIncrement { get; set; }

		public bool IsPrimaryKey { get; set; }
	}

	public class SpecifiedMultiColumnPrimaryKey
	{
		public List<string> Columns { get; set; }
	}
}

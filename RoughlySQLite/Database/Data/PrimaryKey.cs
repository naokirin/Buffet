using System;
using System.Collections.Generic;

namespace RoughlySQLite
{

	class SpecifiedPrimaryKey
	{
		public bool IsAutoIncrement { get; set; }

		public bool IsPrimaryKey { get; set; }
	}

	class SpecifiedMultiColumnPrimaryKey
	{
		public List<string> Columns { get; set; }
	}
}

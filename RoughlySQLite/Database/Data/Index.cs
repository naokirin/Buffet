using System;
using System.Collections.Generic;

namespace RoughlySQLite
{
	class Index
	{
		public string Name { get; set; }
		public List<string> Columns { get; set; }
		public bool Unique { get; set; }

		public Index()
		{
		}

		public Index(string name, List<string> columns, bool unique)
		{
			Name = name;
			Columns = columns;
			Unique = unique;
		}
	}
}


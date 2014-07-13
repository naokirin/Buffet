using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace RoughlySQLite
{

	class ParameterizedString
	{
		public SQLiteType Type { get; set; }
		public string Parameter { get; set; }
		public object Replacing { get; set; }
	}

	class ParameterizedQuery
	{
		public Dictionary<string, ParameterizedString> Parameters { get; private set; }

		public string QueryString { get; private set; }

		public ParameterizedQuery(string query, IEnumerable<ParameterizedString> parameters)
		{
			Parameters = parameters.ToDictionary(x => x.Parameter);
			QueryString = query;
		}
	}
}

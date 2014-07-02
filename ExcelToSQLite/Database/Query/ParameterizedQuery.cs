using System;
using System.Collections.Generic;
using System.Linq;

public class ParameterizedString
{
	public string Parameter { get; set; }
	public string ReplaceString { get; set; }
}

public class ParameterizedQuery
{
	public Dictionary<string, ParameterizedString> Parameters { get; private set; }
	public string QueryString { get; private set; }

	public ParameterizedQuery(string query, IEnumerable<ParameterizedString> parameters)
	{
		Parameters = parameters.ToDictionary(x => x.Parameter);
		QueryString = query;
	}
}
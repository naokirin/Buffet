using System;
using System.Collections.Generic;

namespace Crocell
{
	[AttributeUsage(AttributeTargets.Property)]
	public class IndexedColumnAttribute : ColumnAttribute
	{
		IEnumerable<int> Range { get; set; }

		public IndexedColumnAttribute(string name, IEnumerable<int> range) : base(name)
		{
			Name = name;
			Range = range;
		}
	}
}


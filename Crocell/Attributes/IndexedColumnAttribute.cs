using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Crocell
{
	[AttributeUsage(AttributeTargets.Property)]
	public class IndexedColumnAttribute : ColumnAttribute
	{
		public List<string> Indexes { get; private set; }

		public IndexedColumnAttribute(string name, Type funcType, string funcName) : base(name)
		{
			Name = name;
			MethodInfo method = funcType.GetMethod(funcName);
			var indexes = (List<string>)method.Invoke(null, null);
			Indexes = indexes;
		}
	}
}


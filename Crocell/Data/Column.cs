using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Crocell
{
	class Column
	{
		public ISetter Setter { get; private set; }
		public string Name { get; private set; }
		public string AccessName { get; private set; }
		public bool NotNull { get; private set; }
		public List<string> IndexedNames { get; private set; }

		public Column(PropertyInfo prop)
		{
			Setter = prop.ToSetter();

			var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
			Name = columnAttr == null ? prop.Name : columnAttr.Name;
			AccessName = prop.Name;

			NotNull = prop.GetCustomAttribute<NotNullAttribute>() != null;

			var indexedAttr = prop.GetCustomAttribute<IndexedColumnAttribute>();
			if (indexedAttr != null)
			{
				IndexedNames = indexedAttr.Indexes.Select(x => Name + x).ToList();
			}
		}
	}
}


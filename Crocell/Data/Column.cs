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
		public Type ColumnType { get; private set; }
		public bool NotAllowedEmpty { get; private set; }
		public List<string> IndexedNames { get; private set; }
		public bool IsRowNumber { get; private set; }

		public Column(PropertyInfo prop)
		{
			Setter = prop.ToSetter();

			var rowNumberAttr = prop.GetCustomAttribute<RowNumberAttribute>();
			IsRowNumber = rowNumberAttr != null;

			var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
			Name = columnAttr == null ? prop.Name : columnAttr.Name;
			AccessName = prop.Name;

			NotAllowedEmpty = prop.GetCustomAttribute<AllowedEmptyAttribute>() == null;

			var indexedAttr = prop.GetCustomAttribute<IndexedColumnAttribute>();
			if (indexedAttr != null)
			{
				IndexedNames = indexedAttr.Indexes.Select(x => Name + x).ToList();
			}

			if (IndexedNames == null) ColumnType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
			else
			{
				var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
				ColumnType = type.GenericTypeArguments[0];
			}
		}
	}
}


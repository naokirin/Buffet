using System;
using System.Reflection;

namespace Crocell
{
	class Column
	{
		public ISetter Setter { get; private set; }
		public string Name { get; private set; }
		public string AccessName { get; private set; }
		public Type ColumnType { get; private set; }
		public bool NotNull { get; private set; }

		public Column(PropertyInfo prop)
		{
			Setter = prop.ToSetter();

			var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
			Name = columnAttr == null ? prop.Name : columnAttr.Name;
			AccessName = prop.Name;

			ColumnType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

			NotNull = prop.GetCustomAttribute<NotNullAttribute>() != null;
		}
	}
}


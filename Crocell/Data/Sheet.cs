using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Crocell
{
	class Sheet
	{
		public string Name { get; private set; }
		public List<Column> Columns { get; private set; }

		public Sheet(string name, List<Column> columns)
		{
			Name = name;
			Columns = columns;
		}

		public static Sheet GetSheet(Type t)
		{
			var sheetAttr = (SheetAttribute)t.GetAttribute(typeof(SheetAttribute));
			var sheetName = sheetAttr != null ? sheetAttr.Name : t.Name;

			var properties = from property in t.GetRuntimeProperties()
					where (property.GetMethod != null && property.GetMethod.IsPublic)
				|| (property.SetMethod != null && property.SetMethod.IsPublic)
				|| (property.GetMethod != null && property.GetMethod.IsStatic)
				|| (property.SetMethod != null && property.SetMethod.IsStatic)
				select property;

			var columns = new List<Column>();
			foreach(var property in properties)
			{
				var ignore = property.GetCustomAttributes(typeof(IgnoredColumnAttribute), true).Any();

				if (property.CanWrite && !ignore)
				{
					columns.Add(new Column(property));
				}
			}

			return new Sheet(sheetName, columns);
		}
	}
}


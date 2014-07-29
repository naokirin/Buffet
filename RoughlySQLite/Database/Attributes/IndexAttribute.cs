using System;
using System.Collections.Generic;

namespace RoughlySQLite
{
	public enum Order
	{
		Asc,
		Desc
	}


	public class IndexedColumn
	{
		public string ColumnName { get; private set; }
		public Order Order { get; private set; }

		public IndexedColumn(string columnName, Order order = Order.Asc)
		{
			ColumnName = columnName;
			Order = order;
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class IndexAttribute : Attribute
	{
		public string Name { get; set; }

		public Order Order { get; set; }

		public virtual bool Unique { get; set; }

		public IndexAttribute()
		{
		}

		public IndexAttribute(string name, Order order)
		{
			Name = name;
			Order = order;
		}
	}


	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class MultiColumnIndexAttribute : Attribute
	{
		public string Name { get; set; }

		public virtual bool Unique { get; set; }

		public string[] Columns { get; set; }

		// TODO: specified ordering not string but enum
		public MultiColumnIndexAttribute(string name, string[] columns)
		{
			Name = name;
			Columns = columns;
		}
	}
}

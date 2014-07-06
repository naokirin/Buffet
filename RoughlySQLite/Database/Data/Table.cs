using System;
using System.Collections.Generic;
using System.Linq;

namespace RoughlySQLite
{
	class Table
	{
		public string TableName { get; private set; }

		public List<Column> Columns { get; private set; }

		public SpecifiedMultiColumnPrimaryKey SpecifiedMultiColumnPrimaryKey { get; private set; }

		public List<SpecifiedMultiColumnForeignKey> SpecifiedMultiColumnForeignKeys { get; private set; }

		public Table(string tableName, List<Column> columns, SpecifiedMultiColumnPrimaryKey multiColumnPrimaryKey, List<SpecifiedMultiColumnForeignKey> multiColumnForeignKeys)
		{
			TableName = tableName;
			Columns = columns;
			SpecifiedMultiColumnPrimaryKey = multiColumnPrimaryKey;
			SpecifiedMultiColumnForeignKeys = multiColumnForeignKeys;
		}
	}
}

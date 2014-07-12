using System;
using System.Threading.Tasks;

#if NET45
using SQLiteConnection = System.Data.SQLite.SQLiteConnection;
using SQLiteException = System.Data.SQLite.SQLiteException;
#else
using SQLiteConnection = Mono.Data.Sqlite.SqliteConnection;
using SQLiteException = Mono.Data.Sqlite.SqliteException;
#endif

namespace RoughlySQLite
{
	class NonQeuryCommand
	{
		protected Type TableType { get; set; }

		public NonQeuryCommand(Type t)
		{
			TableType = t;
		}
	}
}


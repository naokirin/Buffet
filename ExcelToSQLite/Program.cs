using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Linq;
using Mono.Data.Sqlite;

class Program
{
	static void Main(string[] args)
	{
		string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

		var dbFileName = folder + "/sample.sqlite";
		SqliteConnectionProvider.DeleteDBFile(dbFileName);
		var db = new SqliteConnectionProvider(dbFileName);
		using(var connection = db.GetOpenConnection())
		{
			connection.CreateTable<Item>();
			connection.CreateTable<TreasureBox>();
			connection.CreateTable<TreasureBoxItemSet>();
		}
	}
}
using NUnit.Framework;
using System;
using ClosedXML.Excel;
using Crocell;
using System.IO;

using System.Linq;
using System.Collections.Generic;


namespace CrocellTest
{
	[Sheet("one_column", DefinedColumn="@start")]
	class OneColumnSheet
	{
		[Column("column")]
		public string Column { get; set; }
	}

	[Sheet("two_column", DefinedColumn="@start")]
	class TwoColumnSheet
	{
		[Column("column1")]
		public string Column { get; set; }

		public string Column2 { get; set; }
	}

	[Sheet("comment_sheet", DefinedColumn="@start", StartingComments=new string[]{ "#" })]
	class CommentSheet
	{
		public string Column { get; set; }
	}

	[Sheet("indexed_column_sheet", DefinedColumn="@start")]
	class IndexedColumnSheet
	{
		public static List<string> Indexed()
		{
			return Enumerable.Range(0, 3).Select(x => x.ToString()).ToList();
		}

		[IndexedColumn("column", typeof(IndexedColumnSheet), "Indexed")]
		public List<string> Column { get; set; }
	}

	[Sheet("date_time_sheet", DefinedColumn="@start")]
	class DateTimeSheet
	{
		public DateTime Time { get; set; }
	}

	[Sheet("not_found")]
	class NotFoundSheet
	{
	}


	[TestFixture]
	public class CrocellTest
	{
		public static string GetTempFileName()
		{
			#if NETFX_CORE
			var name = Guid.NewGuid () + ".sqlite";
			return Path.Combine (Windows.Storage.ApplicationData.Current.LocalFolder.Path, name);
			#else
			return Path.GetTempFileName();
			#endif
		}

		[Test]
		public void TestReadOneColumnSheet()
		{
			using(var wb = new XLWorkbook())
			{
				var ws = wb.Worksheets.Add("one_column");
				ws.Cell("A1").SetValue("@start");
				ws.Cell("B1").SetValue("column");
				ws.Cell("B2").SetValue("1");
				ws.Cell("B3").SetValue("2");

				var data = wb.ReadSheet<OneColumnSheet>();
				Assert.That(data[0].Column, Is.EqualTo("1"));
				Assert.That(data[1].Column, Is.EqualTo("2"));
			}
		}

		[Test]
		public void TestReadTwoColumnSheet()
		{
			using(var wb = new XLWorkbook())
			{
				var ws = wb.Worksheets.Add("two_column");
				ws.Cell("A2").SetValue("@start");
				ws.Cell("B2").SetValue("column1");
				ws.Cell("C2").SetValue("Column2");

				ws.Cell("B3").SetValue("1");
				ws.Cell("C3").SetValue("b3");

				ws.Cell("B4").SetValue("2");
				ws.Cell("C4").SetValue("b4");

				var data = wb.ReadSheet<TwoColumnSheet>();
				Assert.That(data[0].Column, Is.EqualTo("1"));
				Assert.That(data[0].Column2, Is.EqualTo("b3"));

				Assert.That(data[1].Column, Is.EqualTo("2"));
				Assert.That(data[1].Column2, Is.EqualTo("b4"));
			}
		}

		[Test]
		public void TestReadCommentSheet()
		{
			using(var wb = new XLWorkbook())
			{
				var ws = wb.Worksheets.Add("comment_sheet");
				ws.Cell("A1").SetValue("@start");
				ws.Cell("B1").SetValue("Column");

				ws.Cell("A2").SetValue("#comment out");
				ws.Cell("B2").SetValue("1");
				ws.Cell("B3").SetValue("2");

				var data = wb.ReadSheet<CommentSheet>();

				Assert.That(data.Count, Is.EqualTo(1));
				Assert.That(data[0].Column, Is.EqualTo("2"));
			}
		}

		[Test]
		public void TestReadIndexedColumnSheet()
		{
			using(var wb = new XLWorkbook())
			{
				var ws = wb.Worksheets.Add("indexed_column_sheet");
				ws.Cell("A1").SetValue("@start");
				ws.Cell("B1").SetValue("column0");
				ws.Cell("B2").SetValue("0");
				ws.Cell("C1").SetValue("column1");
				ws.Cell("C2").SetValue("1");
				ws.Cell("D1").SetValue("column2");
				ws.Cell("D2").SetValue("2");

				var data = wb.ReadSheet<IndexedColumnSheet>();
				Assert.That(data[0].Column[0], Is.EqualTo("0"));
				Assert.That(data[0].Column[1], Is.EqualTo("1"));
				Assert.That(data[0].Column[2], Is.EqualTo("2"));
			}
		}

		[Test]
		public void TestReadDateTimeSheet()
		{
			using(var wb = new XLWorkbook())
			{
				var ws = wb.Worksheets.Add("date_time_sheet");
				ws.Cell("A1").SetValue("@start");
				ws.Cell("B1").SetValue("Time");
				ws.Cell("B2").SetValue("2014/07/07 10:10:10");
				ws.Cell("B3").SetValue("2014/10/1");

				var data = wb.ReadSheet<DateTimeSheet>();
				Assert.That(data[0].Time, Is.EqualTo(DateTime.Parse("2014/07/07 10:10:10")));
				Assert.That(data[1].Time, Is.EqualTo(DateTime.Parse("2014/10/1")));
			}
		}

		[Test]
		public void TestFailedToReadNotFoundSheet()
		{
			using(var wb = new XLWorkbook())
			{
				Assert.That(() => wb.ReadSheet<NotFoundSheet>(),
					Throws.Exception.TypeOf<NotFoundSheetException>());
			}
		}
	}
}


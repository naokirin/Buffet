using NUnit.Framework;
using System;
using ClosedXML.Excel;
using Crocell;
using System.IO;
using System.Linq;

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
	}
}


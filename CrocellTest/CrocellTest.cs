using NUnit.Framework;
using System;
using ClosedXML.Excel;
using Crocell;
using System.IO;
using System.Linq;

namespace CrocellTest
{
	[Sheet("one_column")]
	class OneColumnSheet
	{
		[Column("column")]
		public string Column { get; set; }
	}

	[Sheet("two_column")]
	class TwoColumnSheet
	{
		[Column("column1")]
		public string Column { get; set; }

		public string Column2 { get; set; }
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
				ws.Cell("A1").SetValue("column");
				ws.Cell("A2").SetValue("1");
				ws.Cell("A3").SetValue("2");

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
				ws.Cell("A2").SetValue("column1");
				ws.Cell("B2").SetValue("Column2");

				ws.Cell("A3").SetValue("1");
				ws.Cell("B3").SetValue("b3");

				ws.Cell("A4").SetValue("2");
				ws.Cell("B4").SetValue("b4");

				var data = wb.ReadSheet<TwoColumnSheet>();
				Assert.That(data[0].Column, Is.EqualTo("1"));
				Assert.That(data[0].Column2, Is.EqualTo("b3"));

				Assert.That(data[1].Column, Is.EqualTo("2"));
				Assert.That(data[1].Column2, Is.EqualTo("b4"));
			}
		}
	}
}


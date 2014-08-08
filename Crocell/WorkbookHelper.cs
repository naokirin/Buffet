using System;
using System.Linq;
using System.Collections.Generic;
using ClosedXML.Excel;

namespace Crocell
{
	public static class WorkbookHelper
	{
		public static List<T> ReadSheet<T>(this XLWorkbook workbook) where T : new()
		{
			var sheet = Sheet.GetSheet(typeof(T));
			IXLWorksheet worksheet = null;
			workbook.Worksheets.TryGetWorksheet(sheet.Name, out worksheet);

			if (worksheet == null) return new List<T>();

			var rows = worksheet.Rows();
			if (rows == null || !rows.Any()) return new List<T>();

			var dict = new Dictionary<string, Column>();
			var data = new List<T>();

			bool defined = false;
			rows.ForEach(row =>
			{
				var firstCell = row.FirstCell();
				var str = firstCell.GetString();
				if (sheet.StartingComments.Any(str.StartsWith)) return;
				if (!defined)
				{
					if (str != null && str == sheet.DefinedColumn)
					{
						row.Cells().Select(cell =>
						{
							var column = sheet.Columns.Find(x => x.Name == cell.GetString());
							var index = cell.Address.ColumnLetter;
							return new { column, index };
						}).ForEach(d =>
						{
							if (d.column != null)
							{
								dict.Add(d.index, d.column);
							}
						});
						defined = true;
					}
					return;
				}

				var obj = new T();

				dict.ForEach(x => 
				{
					var cell = row.Cells().FirstOrDefault(c => c.Address.ColumnLetter == x.Key);
					if (cell != null || (cell == null && !x.Value.NotNull))
					{
						var column = x.Value;
						var pi = typeof(T).GetProperty(column.AccessName);

						// TODO: 正しい型で取得する
						// TODO: 空のセルを許容する場合を考慮する
						pi.SetValue(obj, cell.GetString());
					}
					else if (cell == null && x.Value.NotNull)
					{
						throw new NotAllowingNullException();
					}
				});

				data.Add(obj);
			});

			return data;
		}
	}
}


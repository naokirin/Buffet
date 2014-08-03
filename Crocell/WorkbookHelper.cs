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

			var dict = new Dictionary<int, string>();
			var data = new List<T>();

			bool defined = false;
			rows.ForEach(row =>
			{
				if (!defined)
				{
					var firstCell = row.FirstCell();
					var str = firstCell.GetString();
					// 最初に空でなく、#がついていない行が各列の定義の行
					// TODO: 先頭の名前を何にするかを任意に設定できるようにする
					if (str != null && str.Any() && str[0] != '#')
					{
						row.Cells().Select((cell, index) =>
						{
							var column = sheet.Columns.Find(x => x.Name == cell.GetString());
							return new { column, index };
						}).ForEach(d =>
						{
							if (d.column != null)
							{
								dict.Add(d.index, d.column.AccessName);
							}
						});
						defined = true;
					}
					return;
				}

				var obj = new T();
				row.Cells().Select((cell, index) =>
				{
					return new { cell, index };
				}).Where(x => x.cell != null).ForEach(d =>
				{
					var pi = typeof(T).GetProperty(dict[d.index]);

					// TODO: 正しい型で取得する
					// TODO: 空のセルを許容する場合を考慮する
					pi.SetValue(obj, d.cell.GetString());
				});

				data.Add(obj);
			});

			return data;
		}
	}
}


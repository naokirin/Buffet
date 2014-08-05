﻿using System;
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

			var dict = new Dictionary<string, string>();
			var data = new List<T>();

			bool defined = false;
			rows.ForEach(row =>
			{
				if (!defined)
				{
					var firstCell = row.FirstCell();
					var str = firstCell.GetString();

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
								dict.Add(d.index, d.column.AccessName);
							}
						});
						defined = true;
					}
					return;
				}

				var obj = new T();
				row.Cells().Where(x => x != null && dict.ContainsKey(x.Address.ColumnLetter))
					.ForEach(cell =>
				{
					var pi = typeof(T).GetProperty(dict[cell.Address.ColumnLetter]);

					// TODO: 正しい型で取得する
					// TODO: 空のセルを許容する場合を考慮する
					pi.SetValue(obj, cell.GetString());

				});
				data.Add(obj);
			});

			return data;
		}
	}
}


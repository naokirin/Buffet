using System;
using ClosedXML.Excel;

namespace Crocell
{

	enum CellType
	{
		Boolean,
		Double,
		DateTime,
		String,
		TimeSpan
	}

	static class TypeTranslation
	{
		public static CellType ToCellType(this Type t)
		{
			if (t == typeof(Boolean))
			{
				return CellType.Boolean;
			}
			else if (t == typeof(Byte) || t == typeof(UInt16)
			         || t == typeof(SByte) || t == typeof(Int16) || t == typeof(Int32)
			         || t == typeof(UInt32) || t == typeof(Int64)
			         || t == typeof(Single) || t == typeof(Double) || t == typeof(Decimal)
			         || t.IsEnum)
			{
				return CellType.Double;
			}
			else if (t == typeof(DateTime))
			{
				return CellType.DateTime;
			}
			else if (t == typeof(TimeSpan))
			{
				return CellType.TimeSpan;
			}
			else if (t == typeof(String))
			{
				return CellType.String;
			}

			throw new NotSupportedException(String.Format("Type {0} is not supported", t.Name));
		}

		public static object GetCellData(this IXLCell cell, Type t)
		{
			var type = t.ToCellType();

			switch(type)
			{
			case CellType.Boolean:
				return Convert.ChangeType(cell.GetBoolean(), t);
			case CellType.DateTime:
				return Convert.ChangeType(cell.GetDateTime(), t);
			case CellType.Double:
				return Convert.ChangeType(cell.GetDouble(), t);
			case CellType.String:
				return Convert.ChangeType(cell.GetString(), t);
			case CellType.TimeSpan:
				return Convert.ChangeType(cell.GetTimeSpan(), t);
			}

			return null;
		}
	}
}

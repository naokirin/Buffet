using System;

namespace Crocell
{
	public class NotFoundSheetException : Exception
	{
		public string SheetName { get; set; }
		public NotFoundSheetException(string sheet)
		{
			SheetName = sheet;
		}
	}
}


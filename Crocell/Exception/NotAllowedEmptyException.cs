using System;

namespace Crocell
{
	public class NotAllowedEmptyException : Exception
	{
		public int RowNumber { get; private set; }
		public string ColumnLetter { get; private set; }

		public NotAllowedEmptyException(string columnLetter, int rowNumber)
		{
			RowNumber = rowNumber;
			ColumnLetter = columnLetter;
		}
	}
}


using System;

[Serializable]
public class NotAllowedTypeWithAutoIncrementException : Exception
{
	public NotAllowedTypeWithAutoIncrementException() : base() { }
	public NotAllowedTypeWithAutoIncrementException(string message) : base(message) { }
	public NotAllowedTypeWithAutoIncrementException(string message, System.Exception inner) : base(message, inner) { }
	public NotAllowedTypeWithAutoIncrementException(string tableName, string columnName, Type columnType)
		: base("Column " + tableName + "." + columnName + " declare with AutoIncrement but not allowed type " + columnType.FullName) { }

	protected NotAllowedTypeWithAutoIncrementException(System.Runtime.Serialization.SerializationInfo info,
		System.Runtime.Serialization.StreamingContext context) { }
}

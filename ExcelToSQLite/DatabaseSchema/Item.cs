using System;

[Table("items")]
public class Item
{
	[PrimaryKey(IsAutoIncrement = true)]
	public int Id { get; set; }

	[NotNull]
	public string IconId { get; set; }
}

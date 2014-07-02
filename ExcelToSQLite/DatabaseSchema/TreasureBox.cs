using System;
using System.Collections.Generic;

[Table("treasure_boxes")]
public class TreasureBox
{
	[PrimaryKey]
	public string Id { get; set; }

	[NotNull]
	public string IconId { get; set; }

	[Ignore]
	public List<string> ItemIds { get; set; }
}

[Table("treasure_box_item_sets")]
public class TreasureBoxItemSet
{
	[PrimaryKey]
	[ForeignKey(typeof(Item), "Id", OnDeleteAction=OnDeleteAction.Restrict, OnUpdateAction=OnUpdateAction.Restrict)]
	public string ItemId { get; set; }

	[PrimaryKey]
	[ForeignKey(typeof(TreasureBox), "Id", OnDeleteAction=OnDeleteAction.Cascade, OnUpdateAction=OnUpdateAction.Restrict)]
	public string TreasureBoxId { get; set; }
}
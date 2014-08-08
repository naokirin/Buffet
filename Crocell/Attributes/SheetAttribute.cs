using System;

namespace Crocell
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
	public class SheetAttribute : Attribute
	{
		public string Name { get; set; }
		public string DefinedColumn { get; set; }
		public string[] StartingComments { get; set; }

		public SheetAttribute(string name)
		{
			Name = name;
		}
	}
}


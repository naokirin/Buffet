using System;

namespace Crocell
{
	[AttributeUsage(AttributeTargets.Class)]
	public class SheetAttribute : Attribute
	{
		public string Name { get; set; }

		public SheetAttribute(string name)
		{
			Name = name;
		}
	}
}


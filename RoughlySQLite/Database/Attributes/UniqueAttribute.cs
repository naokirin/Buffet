using System;

namespace RoughlySQLite
{

	[AttributeUsage(AttributeTargets.Property)]
	public class UniqueAttribute : IndexAttribute
	{
		public override bool Unique
		{
			get { return true; }
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class MultiColumnUniqueAttribute : MultiColumnIndexAttribute
	{
		public override bool Unique
		{
			get { return true; }
		}

		public MultiColumnUniqueAttribute(string name, string[] columns): base(name, columns)
		{}
	}
}

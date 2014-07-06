using System;

namespace RoughlySQLite
{

	[AttributeUsage(AttributeTargets.Property)]
	public class UniqueAttribute : IndexedAttribute
	{
		public override bool Unique
		{
			get { return true; }
			set { }
		}
	}
}

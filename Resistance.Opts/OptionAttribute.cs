using System;
using System.Reflection;

namespace Resistance.Opts
{
	[AttributeUsage( AttributeTargets.Property )]
	public class OptionAttribute : Attribute
	{
		public string Name { get; set; }
		public int Order { get; set; }
		public string HelpText { get; set; }
	}

	[AttributeUsage( AttributeTargets.Property )]
	public class ExtraAttribute : Attribute 
	{

	}

}


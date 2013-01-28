using System;
using System.Reflection;

namespace Resistance.Opts
{
	public class NamedResistanceAttribute : Attribute 
	{
		public string Name { get; set; }
	}

	[AttributeUsage( AttributeTargets.Property )]
	public class CommandLineArgumentAttribute : NamedResistanceAttribute
	{
		public int Order { get; set; }
		public string HelpText { get; set; }
	}
}


using System;
using System.Reflection;

namespace Resistance.Opts
{
	[AttributeUsage( AttributeTargets.Property )]
	public class CommandLineArgumentAttribute : Attribute
	{
		public string Name { get; set; }
		public int Order { get; set; }
		public string HelpText { get; set; }

		public CommandLineArgumentAttribute ()
		{

		}
	}
}


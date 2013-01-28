using System;
using System.Reflection;

namespace Resistance.Opts
{
	[AttributeUsage( AttributeTargets.Class )]
	public class CommandLineGroupHeadingAttribute : NamedResistanceAttribute
	{
	}

	[AttributeUsage( AttributeTargets.Class )]
	public class CommandLineGroupDescriptionAttribute : NamedResistanceAttribute
	{
	}
}

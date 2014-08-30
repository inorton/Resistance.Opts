using System;
using System.Reflection;

namespace Resistance.Opts {

	[AttributeUsage( AttributeTargets.Class )]
	public class CommandLineHelpProviderAttribute : Attribute {
	}
}

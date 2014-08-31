using System;
using System.Reflection;

namespace Resistance.Opts {

	/// <summary>
	/// An attribute specifying that the <c>-h</c> or <c>--help</c> attribute should be added as
	/// one of the command line arguments.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	public class CommandLineHelpProviderAttribute : Attribute {
	}
}

using System;
using System.Reflection;
using Mono.Options;

namespace Resistance.Opts
{
	public class HelpRequestedArgument : BadCommandLineArguments 
	{
		public OptHost OptionHost { get; set; }
	}
}
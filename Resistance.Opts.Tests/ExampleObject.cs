using System;
using NUnit.Framework;
using Resistance.Opts;
using System.Collections.Generic;

namespace Resistance.Opts.Tests
{
	[CommandLineHelpProvider]
	[HelpMessage("This is an example group",
	             "Example groups might be the only command line group in a program. Or your program may have several groups",
	             "Each one of these lines will be in a nicely word wrapped paragraph all of it's own",
	             HelpMessage.Options,
	             "You can place more text after the options like this")]
	public class TestObject {

		[CommandLineArgument(HelpText="Enable the test")]
		public bool Enable { get; set; }

		[CommandLineArgument(Name="verbose", HelpText="Be Verbose")]
		public bool Verbose { get; set; }

		[CommandLineArgument(HelpText="Output to {FILE}")]
		public string OutputFile { get; set; }

		[CommandLineArgument(HelpText="Run {NUM} jobs")]
		public int JobCount { get; set; }

		[CommandLineArgument(HelpText="Enable mulitple-threads")]
		public bool Threads { get; set; }
	}
	
}

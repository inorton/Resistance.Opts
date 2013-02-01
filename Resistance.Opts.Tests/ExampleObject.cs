using System;
using NUnit.Framework;
using Resistance.Opts;
using System.Collections.Generic;

namespace Resistance.Opts.Tests
{
	[CommandLineHelpProvider]
	[Help("This might be the only command line group in a program. Or your program may have several groups",
	      "Each one of these lines will be in a nicely word wrapped paragraph all of it's own",
	      "OPTIONS",
	      HelpAttribute.Options,
	      "You can place more text after the options like this")]
	public class ExampleObject {
		[Option(HelpText="Enable the test")]
		public bool Enable { get; set; }

		[Option(Name="verbose", HelpText="Be Verbose")]
		public bool Verbose { get; set; }

		[Option(HelpText="Output to {FILE}")]
		public string OutputFile { get; set; }

		[Option(HelpText="Run {NUM} jobs")]
		public int JobCount { get; set; }

		[Option(HelpText="Enable mulitple-threads")]
		public bool Threads { get; set; }

		[Extra(HelpText="Archive File", Order = 0)]
		public string Archive { get; set; }

		[ExtraList(HelpText="Files to include", Order = 1, MaximumCount=5)]
		public List<string> Files { get; set; }

	}
	
}

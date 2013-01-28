using System;
using NUnit.Framework;
using Resistance.Opts;
using System.Collections.Generic;

namespace Resistance.Opts.Tests
{
	[CommandLineHelpProvider]
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

	[TestFixture]
	public class TestDescrptions
	{
		[Test]
		public void TestBuild ()
		{
			var host = new OptHost<TestObject>();
			host.Opts.WriteOptionDescriptions( Console.Out );

			host.Opts.Parse( new string[] { 
				"--verbose", 
				"--enable",
				"--output-file=myfilename", 
				"--job-count=10"
			} );

			Assert.IsTrue( host.Object.Verbose );
			Assert.IsTrue( host.Object.Enable );
			Assert.AreEqual( 10, host.Object.JobCount );
			Assert.AreEqual( "myfilename", host.Object.OutputFile );
		}

		[Test]
		public void TestTryHelp () {
			var host = new OptHost<TestObject>();
			var extra = new List<string>();
			Assert.IsFalse( host.TryParse( extra, "--help") );
		}

	}
}


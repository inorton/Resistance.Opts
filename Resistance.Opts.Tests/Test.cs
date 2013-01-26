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

	}

	[TestFixture]
	public class TestDescrptions
	{
		[Test]
		public void TestBuild ()
		{
			var o = new TestObject();
			var host = new OptHost();
			host.Build( o, "Options" );
			host.Opts.WriteOptionDescriptions( Console.Out );

			host.Opts.Parse( new string[] { 
				"--verbose", 
				"--enable",
				"--output-file=myfilename", 
				"--job-count=10"
			} );

			Assert.IsTrue( o.Verbose );
			Assert.IsTrue( o.Enable );
			Assert.AreEqual( 10, o.JobCount );
			Assert.AreEqual( "myfilename", o.OutputFile );
		}

		[Test]
		public void TestTryHelp () {
			var o = new TestObject();
			var host = new OptHost();
			host.Build( o );
			var extra = new List<string>();
			Assert.IsFalse( host.TryParse( extra, "--help") );
		}

		[Test]
		[ExpectedException(typeof(HelpRequestedArgument))]
		public void TestThrowHelp () {
			var o = new TestObject();
			var host = new OptHost();
			host.Build( o );
			host.Parse( "--help");
		}
	}
}


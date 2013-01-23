using System;
using NUnit.Framework;
using Resistance.Opts;

namespace Resistance.Opts.Tests
{
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

	[TestFixture()]
	public class TestDescrptions
	{
		[Test()]
		public void TestCase ()
		{
			var o = new TestObject();
			var host = new OptHost();
			var options = host.Build( o, "Options" );
			options.WriteOptionDescriptions( Console.Out );

			options.Parse( new string[] { 
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
	}
}


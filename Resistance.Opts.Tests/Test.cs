using System;
using NUnit.Framework;
using Resistance.Opts;
using System.Collections.Generic;

namespace Resistance.Opts.Tests
{

	[TestFixture]
	public class TestDescrptions
	{
		[Test]
		public void TestBuild ()
		{
			var host = new OptHost<ExampleObject>();
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
			var host = new OptHost<ExampleObject>();
			var extra = new List<string>();
			Assert.IsFalse( host.TryParse( extra, "--help") );
		}



	}
}


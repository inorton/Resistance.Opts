using System;
using System.Reflection;
using System.Collections.Generic;

namespace Resistance.Opts
{
	[AttributeUsage( AttributeTargets.Class )]
	public class HelpAttribute : Attribute
	{
		public List<string> Items { get; private set; }

		public HelpAttribute ( params string[] items)
		{
			Items = new List<string>( items );
		}

		public const string Options = "{OPTIONS}";
	}
	
}

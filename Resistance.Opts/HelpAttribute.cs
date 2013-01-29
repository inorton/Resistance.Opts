using System;
using System.Reflection;
using System.Collections.Generic;

namespace Resistance.Opts
{
	[AttributeUsage( AttributeTargets.Class )]
	public class HelpMessage : Attribute
	{
		public List<string> Items { get; private set; }

		public HelpMessage ( params string[] items)
		{
			Items = new List<string>( items );
		}

		public const string Options = "{OPTIONS}";
	}
	
}

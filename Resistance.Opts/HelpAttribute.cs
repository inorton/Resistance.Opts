using System;
using System.Reflection;
using System.Collections.Generic;

namespace Resistance.Opts {

	/// <summary>
	/// An attribute that specifies how the help message should be formatted.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	public class HelpAttribute : Attribute {

		#region Constants
		/// <summary>
		/// The token used to be replaced with the specified command line options.
		/// </summary>
		public const string Options = "{OPTIONS}";
		#endregion
		#region Properties
		/// <summary>
		/// Gets the list of lines that should be printed for help.
		/// </summary>
		/// <value>A <see cref="T:List`1"/> of lines printed by the help page.</value>
		public List<string> Items { get; private set; }
		#endregion
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="HelpAttribute"/> class with a given list of lines to be printed.
		/// </summary>
		/// <param name="items">The list of lines that are printed when printing the help file.</param>
		public HelpAttribute (params string[] items) {
			Items = new List<string> (items);
		}
		#endregion
	}
}

using System;
using System.Reflection;

namespace Resistance.Opts {

	/// <summary>
	/// The basic implementation of the option attribute: an attribute to assign a command line option
	/// to a certain property.
	/// </summary>
	public class BaseOptionAttribute : Attribute {

		/// <summary>
		/// Get or set the name of the command line option.
		/// </summary>
		/// <value>The name of the command line option.</value>
		public string Name { get; set; }

		public int Order { get; set; }

		/// <summary>
		/// Gets or sets the description that accompanies the command line option.
		/// </summary>
		/// <value>The description that accompanies the command line option.</value>
		public string HelpText { get; set; }
	}

	/// <summary>
	/// The option attribute that is used for most properties (that are not lists).
	/// </summary>
	[AttributeUsage( AttributeTargets.Property )]
	public class OptionAttribute : BaseOptionAttribute {
	
	}

	[AttributeUsage( AttributeTargets.Property )]
	public class ExtraAttribute : BaseOptionAttribute {

	}

	/// <summary>
	/// A special option used for list of values. One can specify the minimum and maximum number of
	/// elements that can be specified.
	/// </summary>
	[AttributeUsage( AttributeTargets.Property )]
	public class ExtraListAttribute : BaseOptionAttribute {

		/// <summary>
		/// Gets or sets the minimum number of elements that must be given with the option.
		/// </summary>
		/// <value>The required number of elements.</value>
		public int MinimumCount { get; set; }

		/// <summary>
		/// Gets or sets the maximum number of elements that must be given with the option.
		/// </summary>
		/// <value>The maximum allowed number of elements.</value>
		public int MaximumCount { get; set; }
	}
}


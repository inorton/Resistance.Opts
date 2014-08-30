using System;
using System.Reflection;
using Mono.Options;

namespace Resistance.Opts {

	public class BadCommandLineArgumentsException : Exception {

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="BadCommandLineArgumentsException"/> is handled.
		/// </summary>
		/// <value><c>true</c> if the exception was handled; otherwise, <c>false</c>.</value>
		public bool Handled { get; set; }
	}
}

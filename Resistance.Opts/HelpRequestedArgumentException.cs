using System;
using System.Reflection;
using Mono.Options;

namespace Resistance.Opts {

	/// <summary>
	/// An exception thrown if help is requested (using the <c>-h</c> argument) instead of
	/// actually executing the program.
	/// </summary>
	public class HelpRequestedArgumentException : BadCommandLineArgumentsException {
	}
}

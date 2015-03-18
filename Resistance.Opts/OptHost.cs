using System;
using System.Runtime;
using System.Linq;
using System.Collections.Generic;
using Mono.Options;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using System.Diagnostics.Contracts;

namespace Resistance.Opts {

	/// <summary>
	/// The generic version of the <see cref="OptHost"/> class.
	/// </summary>
	/// <typeparam name="T">The type of the instance that stores the command line arguments.</typeparam>
	public class OptHost<T> : OptHost
        where T : class, new() {

		/// <summary>
		/// Gets or sets the object that must be configured.
		/// </summary>
		public T Object { get { return settingsObject as T; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:OptHost`1"/> class.
		/// </summary>
		public OptHost () : base () {
			settingsObject = new T ();
		}
	}

	/// <summary>
	/// A host class that handles the options of the given settings object and can write
	/// out a help file.
	/// </summary>
	/// <remarks>
	/// <para>This class performs the reflection and generates a <see cref="OptionSet"/>.</para>
	/// </remarks>
	public class OptHost {

		#region Fields
		/// <summary>
		/// The object that provide the command line settings.
		/// </summary>
		protected object settingsObject;
		#endregion
		#region Properties
		/// <summary>
		/// The options built by this OptHost.
		/// </summary>
		public OptionSet Opts { 
			get {
				return Build (settingsObject);
			}
		}

		/// <summary>
		/// Option related errors and help text are written here.
		/// </summary>
		public TextWriter StdError { get; set; }

		/// <summary>
		///A boolean determining if the process should terminate if an error is thrown at parsing the command
		/// line attributes.
		/// </summary>
		public bool TerminateOnError { get; set; }

		/// <summary>
		/// The termination action that must take place if parsing fails.
		/// </summary>
		/// <value>A <see cref="T:Action`1"/> that will be invoked if parsing failed.</value>
		public Action<int> CustomTerminateProcessAction { get; set; }

		/// <summary>
		/// A utility method that converts a camelcase name to a lower case name where each bump is replaced
		/// with a hyphen and the bump in lower case.
		/// </summary>
		/// <returns>A lower case <see cref="string"/> where each original bump is preceded by a hyphen.</returns>
		/// <param name="ccase">The original string in camelcase (the original name of the property).</param>
		public static string HypenateCamelCase (string ccase) {
			if (ccase.Length > 0x00) {
				StringBuilder sb = new StringBuilder ();
				sb.Append (char.ToLower (ccase.First ()));
				foreach (char c in ccase.Skip (0x01)) {
					if (char.IsUpper (c)) {
						sb.Append ("-");
					}
					sb.Append (char.ToLower (c));
				}
				return sb.ToString ();
			} else {
				return string.Empty;
			}
		}

		/// <summary>
		/// Generate a string that provides help: a list of command line options together with a description.
		/// </summary>
		/// <value>The help.</value>
		public string Help {
			get {
				var tw = new StringWriter ();
				Opts.WriteOptionDescriptions (tw);
				return tw.ToString ();
			}
		}
		#endregion
		#region
		/// <summary>
		/// Build the <see cref="OptionSet"/> of command line arguments based on the given <paramref name="prog"/>.
		/// </summary>
		/// <param name="prog">The object that contains the properties that must be set.</param>
		private static OptionSet Build (object prog) {
			if (prog == null) {
				throw new ArgumentNullException ("prog");
			}
			Contract.EndContractBlock ();
			var ptype = prog.GetType ();
			var oset = new OptionSet ();
			// find the help text if it has any,
			var gp = ptype.GetCustomAttributes (typeof(HelpAttribute), true);
			var before_options = new List<string> ();
			var after_options = new List<string> ();
			bool seen_help_opts = false;
			if (gp != null) {
				// we only want one!
				var help = gp.FirstOrDefault () as HelpAttribute;
				if (help != null) {
					foreach (var h in help.Items) {
						if (h.Contains (HelpAttribute.Options)) {
							seen_help_opts = true;
						} else {
							if (!seen_help_opts) {
								before_options.Add (h);
							} else {
								after_options.Add (h);
							}
						}
					}
				}
			}
			foreach (var bf in before_options)
				oset.Add (bf + Environment.NewLine + Environment.NewLine);
			// build help provider
			var hp = ptype.GetCustomAttributes (typeof(CommandLineHelpProviderAttribute), true);
			if (hp != null && (hp.Length != 0)) {
				oset.Add ("h|help", "Display this message", x => {
					throw new HelpRequestedArgumentException ();
				});
			}
			var opts = new Dictionary<PropertyInfo, OptionAttribute> ();
			var plist = ptype.GetProperties ();
			// properties
			foreach (var pi in plist) {
				var attrs = pi.GetCustomAttributes (typeof(OptionAttribute), true);
				foreach (var a in attrs) {
					var aa = a as OptionAttribute;
					if (aa != null) {
						opts [pi] = aa;
					}
				}
			}
			foreach (var kvp in opts.OrderBy (x =>  {
				return x.Value.Order;
			})) {
				var attr = kvp.Value;
				var prop = kvp.Key;
				var vtype = prop.PropertyType;
				var prototype = HypenateCamelCase (prop.Name);
				if (!string.IsNullOrEmpty (attr.Name))
					prototype = attr.Name;
				if (vtype.IsAssignableFrom (typeof(bool))) {
					if (!prototype.StartsWith ("enable") && string.IsNullOrEmpty (attr.Name))
						prototype = "enable-" + prototype;
					oset.Add (prototype, attr.HelpText, new BoolOptionSetter (prog, prop).BoolAction);
					continue;
				}
				prototype += "=";
				if (vtype.IsAssignableFrom (typeof(int))) {
					oset.Add (prototype, attr.HelpText, new OptionSetter<int> (prog, prop).Action);
					continue;
				}
				if (vtype.IsAssignableFrom (typeof(string))) {
					oset.Add (prototype, attr.HelpText, new OptionSetter<string> (prog, prop).Action);
					continue;
				}
			}
			foreach (var af in after_options) {
				if (!string.IsNullOrEmpty (af))
					oset.Add (Environment.NewLine + af + Environment.NewLine);
			}
			return oset;
		}

		/// <summary>
		/// Parse the given list of command line potential extra arguments are returned as a list.
		/// </summary>
		/// <param name="argv">The list of command line arguments not considered by this <see cref="OptHost"/>.</param>
		/// <exception cref="BadCommandLineArgumentsException">If the given command line arguments are not
		/// properly formatted.</exception>
		List<string> Parse (params string[] argv) {
			Contract.Requires (this.Opts != null);
			try {
				return Opts.Parse (argv);
			} catch (HelpRequestedArgumentException e) {
				var err = StdError ?? Console.Error;
				err.WriteLine (Help);
				e.Handled = true;
				if (TerminateOnError) { 
					if (CustomTerminateProcessAction != null) {
						CustomTerminateProcessAction.Invoke (1);
					} else {
						Environment.Exit (1);
					}
				}
				throw;
			}
		}

		/// <summary>
		/// Tries the parse the given list of command line arguments and add the extra arguments (not considered
		/// by this <see cref="OptHost"/> to the given <paramref name="extraOut"/> list).
		/// </summary>
		/// <returns><c>true</c>, if the given parsing was successful, <c>false</c> otherwise.</returns>
		/// <param name="extraOut">The list to store the additional command line arguments in (if available).</param>
		/// <param name="argv">The given list of command line arguments to parse.</param>
		public bool TryParse (List<string> extraOut, params string[] argv) {
			if (extraOut == null) {
				throw new ArgumentNullException ("extraOut");
			}
			Contract.EndContractBlock ();
			try {
				extraOut.AddRange (Parse (argv));
				return true;
			} catch (BadCommandLineArgumentsException) {
			}
			return false;
		}
		#endregion
	}
}


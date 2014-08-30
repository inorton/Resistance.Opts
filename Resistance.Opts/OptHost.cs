using System;
using System.Runtime;
using System.Linq;
using System.Collections.Generic;
using Mono.Options;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

namespace Resistance.Opts {

	public class OptHost<T> : OptHost
        where T : class, new() {
		/// <summary>
		/// Gets or sets the configuration object.
		/// </summary>
		public T Object { get { return settingsObject as T; } }

		public OptHost () : base () {
			settingsObject = new T ();
		}
	}

	/// <summary>
	/// A host class that handles the options of the given settings object and can write
	/// out a help file.
	/// </summary>
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
		/// Terminate the process if a bad option is supplied.
		/// </summary>
		public bool TerminateOnError { get; set; }

		public Action<int> CustomTerminateProcessAction { get; set; }

		public static string HypenateCamelCase (string ccase) { 
			var sb = new System.Text.StringBuilder ();
			foreach (var c in ccase.ToCharArray()) {
				if (Char.IsUpper (c)) {
					if (sb.Length > 0) {
						sb.Append ("-");
					}
				}
				sb.Append (c);
			}

			return sb.ToString ().ToLower ();
		}

		public string Help {
			get {
				var tw = new StringWriter ();
				Opts.WriteOptionDescriptions (tw);
				return tw.ToString ();
			}
		}
		#endregion
		#region
		OptionSet Build (object prog) {
			if (prog == null)
				throw new ArgumentNullException ("prog");

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
				oset.Add ("h|help", "Display this message", (x) => {
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

			foreach (var kvp in opts.OrderBy( x => { return x.Value.Order; } )) {
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

		List<string> Parse (params string[] argv) {
			try {
				return Opts.Parse (argv);
			} catch (HelpRequestedArgumentException e) {
				var err = StdError != null ? StdError : Console.Error;
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

		public bool TryParse (List<string> extraOut, params string[] argv) {
			if (extraOut == null)
				throw new ArgumentNullException ("extraOut");
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


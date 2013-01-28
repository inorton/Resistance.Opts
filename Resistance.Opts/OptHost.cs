using System;
using System.Runtime;
using System.Linq;
using System.Collections.Generic;
using Mono.Options;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

namespace Resistance.Opts
{
	public class OptHost<T> : OptHost
		where T : class, new()
	{
		/// <summary>
		/// Gets or sets the configuration object.
		/// </summary>
		public T Object { get { return settingsObject as T; } }

		public OptHost() : base () {
			settingsObject = new T();
		}
	}

    public class OptHost
	{
		protected object settingsObject;

		/// <summary>
		/// Set the optional heading for this option group.
		/// </summary>
		public string Heading { get; set; }

		/// <summary>
		/// The options built by this OptHost.
		/// </summary>
		public OptionSet Opts { 
			get {
				return Build ( settingsObject );
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


		public OptHost()
		{
			Heading = "Options";
		}

		public static string HypenateCamelCase (string ccase)
		{ 
			var sb = new System.Text.StringBuilder ();
			foreach (var c in ccase.ToCharArray()) {
				if ( Char.IsUpper(c) ) {
					if ( sb.Length > 0 ) {
						sb.Append("-");
					}
				}
				sb.Append(c);
			}

			return sb.ToString().ToLower();
		}

		public string Help {
			get {
				var tw = new StringWriter();
				Opts.WriteOptionDescriptions( tw );
				return tw.ToString();
			}
		}

		OptionSet Build (object prog)
		{
			if (prog == null)
				throw new ArgumentNullException ("prog");

			var ptype = prog.GetType ();

			var oset = new OptionSet ();

			// find the group heading if any,
			var gp = ptype.GetCustomAttributes (typeof(CommandLineGroupHeadingAttribute), true);
			if (gp != null) {
				if ( gp.Length > 1 ) throw new InvalidOperationException("more than one options heading given");
			}

			if (!string.IsNullOrEmpty (Heading)) {
				oset.Add(Heading);
			}



			// build help provider
			var hp = ptype.GetCustomAttributes (typeof(CommandLineHelpProviderAttribute), true);
			if ( hp != null && (hp.Length != 0)) {
				oset.Add( "h|help", "Display this message", (x) => {
					throw new HelpRequestedArgument();
				});
			}

			var opts = new Dictionary<PropertyInfo, CommandLineArgumentAttribute> ();

			var plist = ptype.GetProperties();

			// properties
			foreach (var pi in plist) {
				var attrs = pi.GetCustomAttributes( typeof(CommandLineArgumentAttribute),true );
				foreach ( var a in attrs ) {
					var aa = a as CommandLineArgumentAttribute;
					if ( aa != null ){
						opts[pi] = aa;
					}
				}
			}

			foreach ( var kvp in opts.OrderBy( x => { return x.Value.Order; } ) )
			{
				var attr = kvp.Value;
				var prop = kvp.Key;
				var vtype = prop.PropertyType;
				var prototype = HypenateCamelCase(prop.Name);
				if ( !string.IsNullOrEmpty( attr.Name ) )
					prototype = attr.Name;

				if ( vtype.IsAssignableFrom(typeof(bool)) ){
					if ( !prototype.StartsWith("enable") && string.IsNullOrEmpty(attr.Name) )
						prototype = "enable-" + prototype;

					oset.Add( prototype, attr.HelpText, new BoolOptionSetter( prog, prop ).BoolAction );
					continue;
				} 

				prototype += "=";

				if ( vtype.IsAssignableFrom(typeof(int)) ){
					oset.Add( prototype, attr.HelpText, new OptionSetter<int>( prog, prop ).Action );
					continue;
				}

				if ( vtype.IsAssignableFrom(typeof(string)) ){
					oset.Add( prototype, attr.HelpText, new OptionSetter<string>( prog, prop ).Action );
					continue;
				}
			}

			return oset;
		}

		List<string> Parse (params string[] argv)
		{
			try {
				return Opts.Parse (argv);
			} catch (HelpRequestedArgument e) {
				var err = StdError != null ? StdError : Console.Error;
				err.WriteLine( Help );
				e.Handled = true;
				if ( TerminateOnError ){ 
					if ( CustomTerminateProcessAction != null )
					{
						CustomTerminateProcessAction.Invoke(1);
					} else {
						Environment.Exit(1);
					}
				}

				throw;
			}
		}

		public bool TryParse( List<string> extraOut, params string[] argv ){
			if ( extraOut == null ) throw new ArgumentNullException( "extraOut" );
			try {
				extraOut.AddRange( Parse ( argv ) );
				return true;
			} catch ( BadCommandLineArguments ) {
			}
			return false;
		}
	}
}


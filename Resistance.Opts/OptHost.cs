using System;
using System.Runtime;
using System.Linq;
using System.Collections.Generic;
using Mono.Options;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;

namespace Resistance.Opts
{
	public class OptionSetter<T> 
	{
		object optionOwner;
		PropertyInfo optionPropertyInfo;

		public OptionSetter (object obj, PropertyInfo prop)
		{
			if ( obj == null ) throw new ArgumentNullException("obj");
			if ( prop == null ) throw new ArgumentNullException("prop");
			optionOwner = obj;
			optionPropertyInfo = prop;
		}

		public virtual void Invoke (object value)
		{
			optionPropertyInfo.SetValue( optionOwner, value, null );
		}

		public Action<T> Action {
			get {
				return delegate(T value) { Invoke(value); };
			}
		}
	}

	public class BoolOptionSetter : OptionSetter<bool> {
		public BoolOptionSetter( object obj, PropertyInfo prop) : base ( obj, prop ) { }
		public Action<string> BoolAction {
			get {
				return delegate(string v) { base.Invoke(!string.IsNullOrEmpty(v)); };
			}
		}
	}

	public class OptHost
	{
		public OptionSet Opts { get; private set; }

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

		public void Build (object grp, string heading)
		{
			var oset = new OptionSet();
			oset.Add( heading );
			Opts = Build ( grp, oset );
		}

		public void Build (object grp)
		{
			Opts = Build(grp, new OptionSet());
		}

		static OptionSet Build (object prog, OptionSet oset)
		{
			if (prog == null)
				throw new ArgumentNullException ("prog");

			if ( oset == null ) oset = new OptionSet();

			var ptype = prog.GetType ();

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

		public List<string> Parse( params string[] argv ) {
			return Opts.Parse( argv );
		}
	}
}


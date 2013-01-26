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

	public class BoolOptionSetter : OptionSetter<bool> {
		public BoolOptionSetter( object obj, PropertyInfo prop) : base ( obj, prop ) { }
		public Action<string> BoolAction {
			get {
				return delegate(string v) { base.Invoke(!string.IsNullOrEmpty(v)); };
			}
		}
	}
	
}

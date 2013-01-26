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
	public class Utils {
		public static OptHost<T> Create<T>(T options)
			where T : class
		{
			if (options == null)
				throw new ArgumentNullException ("options");
			return new OptHost<T> () { OptionSettings = options };
		}
	}
	
}

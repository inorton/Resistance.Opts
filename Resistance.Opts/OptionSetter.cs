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
	
}

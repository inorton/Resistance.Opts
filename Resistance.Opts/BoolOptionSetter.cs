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

	/// <summary>
	/// A special <see cref="T:OptionSetter`1"/> used for <see cref="bool"/> types such
	/// that mentioning the flag is equal to setting the flag on true.
	/// </summary>
	public class BoolOptionSetter : OptionSetter<bool> {

		#region Properties override
		/// <summary>
		/// Get the action to convert a string into a boolean value.
		/// </summary>
		/// <value>A <see cref="T:Action`1"/> that converts the textual value to a boolean and set
		/// the property accordingly.</value>
		public Action<string> BoolAction {
			get {
				return delegate(string v) {
					this.OptionPropertyInfo.SetValue (this.OptionOwner, !string.IsNullOrEmpty (v), null);
				};
			}
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="T:BoolOptionSetter"/> class with the given
		/// <paramref name="obj"/> to which the data must be set and the <paramref name="prop"/>
		/// that specifies the information of the property.
		/// </summary>
		/// <param name="obj">The object to which the given data must be set.</param>
		/// <param name="prop">The <see cref="PropertyInfo"/> of the property to be set.</param>
		/// <exception cref="ArgumentNullException">If the given <paramref name="obj"/> is not effective.</exception>
		/// <exception cref="ArgumentNullException">If the given <paramref name="prop"/> is not effective.</exception>
		/// <exception cref="ArgumentException">If the given <paramref name="prop"/> has no setter.</exception>
		public BoolOptionSetter (object obj, PropertyInfo prop) : base (obj,prop) {
		}
		#endregion
	}
}

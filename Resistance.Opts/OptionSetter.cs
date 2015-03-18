using System;
using System.Reflection;
using System.Diagnostics.Contracts;

namespace Resistance.Opts {

	/// <summary>
	/// A class that sets a property of an object based on a <see cref="OptionAttribute"/> attribute.
	/// </summary>
	/// <typeparam name="T">The type of input on which the propert will be set.</typeparam>
	public class OptionSetter<T> {

		#region Field
		/// <summary>
		/// An object that 
		/// </summary>
		protected object OptionOwner;
		/// <summary>
		/// The data that specifies the property that must be set.
		/// </summary>
		protected PropertyInfo OptionPropertyInfo;
		#endregion
		#region Property
		/// <summary>
		/// Get an <see cref="T:Action`1"/> that will set the property of the option owner with
		/// the given value.
		/// </summary>
		/// <value>A <see cref="T:Action`1"/> that takes as parameter the value to be set.</value>
		public Action<T> Action {
			get {
				return delegate(T value) {
					Invoke (value);
				};
			}
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="T:OptionSetter`1"/> class with the given
		/// <paramref name="obj"/> to which the data must be set and the <paramref name="prop"/>
		/// that specifies the information of the property.
		/// </summary>
		/// <param name="obj">The object to which the given data must be set.</param>
		/// <param name="prop">The <see cref="PropertyInfo"/> of the property to be set.</param>
		/// <exception cref="ArgumentNullException">If the given <paramref name="obj"/> is not effective.</exception>
		/// <exception cref="ArgumentNullException">If the given <paramref name="prop"/> is not effective.</exception>
		/// <exception cref="ArgumentException">If the given <paramref name="prop"/> has no setter.</exception>
		public OptionSetter (object obj, PropertyInfo prop) {
			if (obj == null) {
				throw new ArgumentNullException ("obj");
			}
			if (prop == null) {
				throw new ArgumentNullException ("prop");
			}
			if (!prop.CanWrite) {
				throw new ArgumentException ("prop");
			}
			Contract.EndContractBlock ();
			Contract.Ensures (this.OptionOwner != null);
			Contract.Ensures (this.OptionPropertyInfo != null);
			Contract.Ensures (this.OptionPropertyInfo.CanWrite);
			OptionOwner = obj;
			OptionPropertyInfo = prop;
		}
		#endregion
		#region Methods
		/// <summary>
		/// Sets the specified option of the owner object with the given <paramref name="value"/>
		/// </summary>
		/// <param name="value">The new value of the property of the owner object.</param>
		public virtual void Invoke (object value) {
			OptionPropertyInfo.SetValue (OptionOwner, value, null);
		}
		#endregion
	}
}

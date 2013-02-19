using System;
using System.Reflection;

namespace Resistance.Opts
{
    public class OptionSetter<T>
    {
        protected object OptionOwner;
        protected PropertyInfo OptionPropertyInfo;

        public OptionSetter(object obj, PropertyInfo prop)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (prop == null)
                throw new ArgumentNullException("prop");
            OptionOwner = obj;
            OptionPropertyInfo = prop;
        }

        public virtual void Invoke(object value)
        {
            OptionPropertyInfo.SetValue(OptionOwner, value, null);
        }

        public Action<T> Action
        {
            get
            {
                return delegate(T value)
                {
                    Invoke(value);
                };
            }
        }
    }
	
}

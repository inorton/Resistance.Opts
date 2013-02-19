using System;
using System.Reflection;

namespace Resistance.Opts
{
    public class OptionSetter<T>
    {
        object optionOwner;
        PropertyInfo optionPropertyInfo;

        public OptionSetter(object obj, PropertyInfo prop)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (prop == null)
                throw new ArgumentNullException("prop");
            optionOwner = obj;
            optionPropertyInfo = prop;
        }

        public virtual void Invoke(object value)
        {
            optionPropertyInfo.SetValue(optionOwner, value, null);
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

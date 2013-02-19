using System;
using System.Reflection;

namespace Resistance.Opts
{
    public class BaseOptionAttribute : Attribute
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public string HelpText { get; set; }
    }

    [AttributeUsage( AttributeTargets.Property )]
    public class OptionAttribute : BaseOptionAttribute
    {
	
    }

    [AttributeUsage( AttributeTargets.Property )]
    public class ExtraAttribute : BaseOptionAttribute
    {

    }

    [AttributeUsage( AttributeTargets.Property )]
    public class ExtraListAttribute : BaseOptionAttribute
    {
        public int MinimumCount { get; set; }
        public int MaximumCount { get; set; }
    }
}


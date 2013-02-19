using System;
using System.Reflection;
using Mono.Options;

namespace Resistance.Opts
{
    public class BadCommandLineArguments : Exception
    {
        public bool Handled { get; set; }
    }
    
}

using Cake.Core;
using Cake.Core.IO;

namespace Cake.AzCopy
{
    internal static class Extensions
    {
        internal static bool IsDefined(this string value) {
            return !string.IsNullOrEmpty(value);
        }

        internal static ProcessArgumentBuilder BuildForWindows(this AzCopySettings settings, string source, string destination) {
            var args = new ProcessArgumentBuilder();
            args.AppendSwitchQuoted("/Source", ":", source);
            args.AppendSwitchQuoted("/Dest", ":", destination);
            args = settings.BuildWindowsArguments(args);
            return args;
        }

        internal static ProcessArgumentBuilder BuildForLinux(this AzCopySettings settings, string source, string destination) {
            var args = new ProcessArgumentBuilder();
            args.AppendSwitchQuoted("--source", source);
            args.AppendSwitchQuoted("--destination", destination);
            args = settings.BuildLinuxArguments(args);
            return args;
        }
        
    }
}
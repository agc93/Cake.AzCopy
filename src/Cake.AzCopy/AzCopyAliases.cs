using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.AzCopy
{
    [CakeNamespaceImport("Cake.AzCopy")]
    [CakeAliasCategory("Azure")]
    public static class AzCopyAliases
    {
        [CakeMethodAlias]
        public static void AzCopy(this ICakeContext ctx, string source, string destination) {
            if (ctx.Environment.Platform.Family == PlatformFamily.OSX) throw new NotSupportedException("AzCopy is not supported on macOS");
            ctx.AzCopy(source, destination, new AzCopySettings());
        }

        [CakeMethodAlias]
        public static void AzCopy(this ICakeContext ctx, string source, string destination, AzCopySettings settings) {
            if (ctx.Environment.Platform.Family == PlatformFamily.OSX) throw new NotSupportedException("AzCopy is not supported on macOS");
            var runner = new AzCopyRunner(ctx);
            runner.RunTool(source, destination, settings);
        }

        [CakeMethodAlias]
        public static void AzCopy(this ICakeContext ctx, string source, string destination, Action<AzCopySettings> configure) {
            if (ctx.Environment.Platform.Family == PlatformFamily.OSX) throw new NotSupportedException("AzCopy is not supported on macOS");
            var settings = new AzCopySettings();
            configure?.Invoke(settings);
            ctx.AzCopy(source, destination, settings);
        }
    }
}

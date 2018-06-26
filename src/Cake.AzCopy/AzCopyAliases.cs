using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.AzCopy
{
    [CakeNamespaceImport("Cake.AzCopy")]
    [CakeAliasCategory("Azure")]
    public static class AzCopyAliases
    {
        /// <summary>
        /// Invokes AzCopy with the given source and destination, using all the defaults
        /// </summary>
        /// <param name="ctx">The Cake context.</param>
        /// <param name="source">The source file(s) or path(s).</param>
        /// <param name="destination">The destination path.</param>
        [CakeMethodAlias]
        public static void AzCopy(this ICakeContext ctx, string source, string destination) {
            if (ctx.Environment.Platform.Family == PlatformFamily.OSX) throw new NotSupportedException("AzCopy is not supported on macOS");
            ctx.AzCopy(source, destination, new AzCopySettings());
        }

        /// <summary>
        /// Invokes AzCopy with the given source and destination, using the given settings
        /// </summary>
        /// <param name="ctx">The Cake context.</param>
        /// <param name="source">The source file(s) or path(s).</param>
        /// <param name="destination">The destination path.</param>
        /// <param name="settings">The <see cref="AzCopySettings"/> to use when copying</param>
        [CakeMethodAlias]
        public static void AzCopy(this ICakeContext ctx, string source, string destination, AzCopySettings settings) {
            if (ctx.Environment.Platform.Family == PlatformFamily.OSX) throw new NotSupportedException("AzCopy is not supported on macOS");
            var runner = new AzCopyRunner(ctx);
            runner.RunTool(source, destination, settings);
        }

        /// <summary>
        /// Invokes AzCopy with the given source and destination, using the given settings
        /// </summary>
        /// <param name="ctx">The Cake context.</param>
        /// <param name="source">The source file(s) or path(s).</param>
        /// <param name="destination">The destination path.</param>
        /// <param name="settings">The settings configurator.</param>
        [CakeMethodAlias]
        public static void AzCopy(this ICakeContext ctx, string source, string destination, Action<AzCopySettings> configure) {
            if (ctx.Environment.Platform.Family == PlatformFamily.OSX) throw new NotSupportedException("AzCopy is not supported on macOS");
            var settings = new AzCopySettings();
            configure?.Invoke(settings);
            ctx.AzCopy(source, destination, settings);
        }
    }
}

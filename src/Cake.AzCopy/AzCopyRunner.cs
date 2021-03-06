using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.AzCopy
{
    /// <summary>
    /// The AzCopy runner.
    /// </summary>
    public class AzCopyRunner : Tool<AzCopySettings>
    {
        private readonly ICakeLog _log;
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <c>AzCopyRunner</c> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="log">The log.</param>
        public AzCopyRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools, ICakeLog log)
            : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
            _log = log;
        }

        /// <summary>
        /// Initializes a new instance of the <c>AzCopyRunner</c> class.
        /// </summary>
        /// <param name="ctx">The context.</param>
        public AzCopyRunner(ICakeContext ctx)
            : this(ctx.FileSystem, ctx.Environment, ctx.ProcessRunner, ctx.Tools, ctx.Log)
        {

        }
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            yield return "AzCopy.exe";
            yield return "azcopy";
            yield return "AzCopy";
        }

        protected override string GetToolName() => "AzCopy";

        public void RunTool(string source, string destination, AzCopySettings settings) {
            var args = _environment.Platform.Family == PlatformFamily.Windows 
                ? settings.BuildForWindows(source, destination)
                : settings.BuildForLinux(source, destination);
            if (_log.Verbosity == Verbosity.Verbose || _log.Verbosity == Verbosity.Diagnostic && _environment.Platform.Family == PlatformFamily.Linux) {
                args.Append("--verbose");
            }
            Run(settings, args);
        } 
    }
}
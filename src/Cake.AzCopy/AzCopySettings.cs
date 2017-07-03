using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.AzCopy
{
    public class AzCopySettings : ToolSettings
    {
        public string Pattern {get;set;}
        public string DestinationKey {get;set;}
        public string SourceKey {get;set;}
        public bool Recursive {get;set;}
        public string DestinationSAS {get;set;}
        public string SourceSAS {get;set;}
        public BlobType? BlobType {get;set;}
        public bool UseChecksum {get;set;}
        //public bool UseSnapshots {get;set;}

        public FileHandling? FileHandlingBehaviour {get;set;}

        public char? Delimiter {get;set;}
        public int ConcurrentOperations {get;set;}
        public string TargetContentType {get;set;}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>This value is ignored when running on Linux</remarks>
        public PayloadFormat PayloadFormat {get;set;}

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>This value is ignored when running on Linux</remarks>
        public FilePath LogFile {get;set;}
        public List<FilePath> ParameterFiles {get;set;} = new List<FilePath>();

        public ProcessArgumentBuilder BuildWindowsArguments(ProcessArgumentBuilder args) {
            //var args = new ProcessArgumentBuilder();
            args.Append("/Y");
            if (Pattern.IsDefined()) {
                args.AppendSwitchQuoted("/Pattern", ":", Pattern);
            }
            if (DestinationKey.IsDefined()) {
                args.AppendSwitchQuotedSecret("/DestKey", ":", DestinationKey);
            }
            if (DestinationSAS.IsDefined()) {
                args.AppendSwitchQuotedSecret("/DestSAS", ":", DestinationSAS);
            }
            if (SourceKey.IsDefined()) {
                args.AppendSwitchQuotedSecret("/SourceKey", ":", SourceKey);
            }
            if (SourceSAS.IsDefined()) {
                args.AppendSwitchQuotedSecret("/SourceSAS", ":", SourceSAS);
            }
            if (Recursive) {
                args.Append("/S");
            }
            if (BlobType != null) {
                args.AppendSwitchQuoted("/BlobType", ":", BlobType.ToString());
            }
            if (UseChecksum) {
                args.Append("/CheckMD5");
            }
            if (LogFile != null) {
                args.AppendSwitchQuoted("/V", ":", LogFile.FullPath);
            }
            if (ParameterFiles.Any()) {
                foreach (var file in ParameterFiles)
                {
                    args.AppendSwitchQuoted("@", ":", file.FullPath);
                }
            }
            if (FileHandlingBehaviour != null) {
                if (FileHandlingBehaviour.Value.HasFlag(FileHandling.ArchiveOnly)) {
                    args.Append("/A");
                }
                if (FileHandlingBehaviour.Value.HasFlag(FileHandling.ExcludeNewerSource)) {
                    args.Append("/XN");
                }
                if (FileHandlingBehaviour.Value.HasFlag(FileHandling.ExcludeOlderSource)) {
                    args.Append("/XO");
                }
                if (FileHandlingBehaviour.Value.HasFlag(FileHandling.UpdateLastModified)) {
                    args.Append("/MT");
                }
            }
            if (Delimiter != null) {
                args.AppendSwitchQuoted("/Delimiter", ":", Delimiter.ToString());
            }
            if (ConcurrentOperations != 0) {
                args.AppendSwitchQuoted("/NC", ":", ConcurrentOperations.ToString());
            }
            if (TargetContentType.IsDefined()) {
                args.AppendSwitchQuoted("/SetContentType", ":", TargetContentType);
            }
            if (PayloadFormat != PayloadFormat.Default) {
                args.AppendSwitchQuoted("/PayloadFormat", ":", PayloadFormat.ToString());
            }
            return args;
        }

        public ProcessArgumentBuilder BuildLinuxArguments(ProcessArgumentBuilder args) {
            args.Append("--quiet"); // equivalent to /Y for some reason
            if (Pattern.IsDefined()) {
                args.AppendSwitchQuoted("--include", Pattern);
            }
            if (DestinationKey.IsDefined()) {
                args.AppendSwitchQuotedSecret("--dest-key", DestinationKey);
            }
            if (DestinationSAS.IsDefined()) {
                args.AppendSwitchQuotedSecret("--dest-sas", DestinationSAS);
            }
            if (SourceKey.IsDefined()) {
                args.AppendSwitchQuotedSecret("--source-key", SourceKey);
            }
            if (SourceSAS.IsDefined()) {
                args.AppendSwitchQuotedSecret("--source-sas", SourceSAS);
            }
            if (Recursive) {
                args.Append("--recursive");
            }
            if (BlobType != null) {
                args.AppendSwitch("--blob-type", BlobType.ToString().ToLower());
            }
            if (UseChecksum) {
                args.Append("--check-md5");
            }
            if (ParameterFiles.Any()) {
                foreach (var file in ParameterFiles)
                {
                    args.AppendSwitchQuoted("--config-file", file.FullPath);
                }
            }
            if (FileHandlingBehaviour != null) {
                if (FileHandlingBehaviour.Value.HasFlag(FileHandling.ExcludeNewerSource)) {
                    args.Append("--exclude-newer");
                }
                if (FileHandlingBehaviour.Value.HasFlag(FileHandling.ExcludeOlderSource)) {
                    args.Append("--exclude-older");
                }
                if (FileHandlingBehaviour.Value.HasFlag(FileHandling.UpdateLastModified)) {
                    args.Append("--preserve-last-modified-time");
                }
            }
            if (Delimiter != null) {
                args.AppendSwitchQuoted("--delimiter", Delimiter.ToString());
            }
            if (ConcurrentOperations != 0) {
                args.AppendSwitchQuoted("--parallel-level", ConcurrentOperations.ToString());
            }
            if (TargetContentType.IsDefined()) {
                args.AppendSwitchQuoted("--set-content-type", TargetContentType);
            }
            return args;
        }
    }
}
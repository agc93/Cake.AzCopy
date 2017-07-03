using Cake.Core.IO;

namespace Cake.AzCopy
{
    public static class AzCopySettingsExtensions
    {
        public static AzCopySettings UsePattern(this AzCopySettings settings, string pattern) {
            settings.Pattern = pattern;
            return settings;
        }

        public static AzCopySettings UseDestinationAccountKey(this AzCopySettings settings, string accountKey) {
            settings.DestinationKey = accountKey;
            return settings;
        }

        public static AzCopySettings UseSourceAccountKey(this AzCopySettings settings, string accountKey) {
            settings.SourceKey = accountKey;
            return settings;
        }

        public static AzCopySettings UseDestinationSignature(this AzCopySettings settings, string sas) {
            settings.DestinationSAS = sas;
            return settings;
        }

        public static AzCopySettings UseSourceSignature(this AzCopySettings settings, string sas) {
            settings.SourceSAS = sas;
            return settings;
        }

        public static AzCopySettings CopyRecursively(this AzCopySettings settings, bool recursive = true) {
            settings.Recursive = recursive;
            return settings;
        }

        public static AzCopySettings SetBlobType(this AzCopySettings settings, string blobType) {
            if (System.Enum.TryParse(blobType, true, out BlobType type)) {
                settings.BlobType = type;
            }
            return settings;
        }

        public static AzCopySettings SetBlobType(this AzCopySettings settings, BlobType type) {
            settings.BlobType = type;
            return settings;
        }

        public static AzCopySettings EnableChecksums(this AzCopySettings settings, bool enablemd5 = true) {
            settings.UseChecksum = enablemd5;
            return settings;
        }

        public static AzCopySettings LogToFile(this AzCopySettings settings, FilePath logFile) {
            settings.LogFile = logFile;
            return settings;
        }

        public static AzCopySettings AddResponseFile(this AzCopySettings settings, FilePath responseFile) {
            settings.ParameterFiles.Add(responseFile);
            return settings;
        }

        public static AzCopySettings SetFileBehaviour(this AzCopySettings settings, FileHandling behaviour) {
            settings.FileHandlingBehaviour = behaviour;
            return settings;
        }

        public static AzCopySettings UseDelimiter(this AzCopySettings settings, char delimiter) {
            settings.Delimiter = delimiter;
            return settings;
        }

        public static AzCopySettings SetConcurrentOperationsCount(this AzCopySettings settings, int count) {
            if (count <= 512) {
                settings.ConcurrentOperations = count;
            }
            return settings;
        }

        public static AzCopySettings SetContentType(this AzCopySettings settings, string contentType) {
            settings.TargetContentType = contentType;
            return settings;
        }

        public static AzCopySettings SetPayloadFormat(this AzCopySettings settings, PayloadFormat format) {
            settings.PayloadFormat = format;
            return settings;
        }

        public static AzCopySettings SetPayloadFormat(this AzCopySettings settings, string payloadFormat) {
            if (System.Enum.TryParse(payloadFormat, true, out PayloadFormat format)) {
                settings.PayloadFormat = format;
            }
            return settings;
        }
    }
}
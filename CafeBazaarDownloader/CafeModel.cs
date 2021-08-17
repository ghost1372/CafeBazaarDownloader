using System.Collections.Generic;

namespace CafeBazaarDownloader
{
    public class CafeModel
    {
        public class Properties
        {
            public int statusCode { get; set; }
            public int maxAge { get; set; }
            public string etag { get; set; }
            public string errorMessage { get; set; }
            public int errorCode { get; set; }
        }

        public class AppDownloadInfoReply
        {
            public string token { get; set; }
            public string hashCode { get; set; }
            public List<string> cdnPrefix { get; set; }
            public string ipAddress { get; set; }
            public string packageSize { get; set; }
            public int versionCode { get; set; }
            public bool multiConnectionDownload { get; set; }
            public List<object> compatibleDevices { get; set; }
            public List<object> packageDiffs { get; set; }
            public List<object> baseReferrers { get; set; }
            public bool hasSplits { get; set; }
            public bool hasAdditionalFiles { get; set; }
            public List<object> splits { get; set; }
            public List<object> additionalFiles { get; set; }
        }

        public class SingleReply
        {
            public AppDownloadInfoReply appDownloadInfoReply { get; set; }
        }

        public class RootObject
        {
            public Properties properties { get; set; }
            public object internalProperties { get; set; }
            public SingleReply singleReply { get; set; }
        }
    }
}

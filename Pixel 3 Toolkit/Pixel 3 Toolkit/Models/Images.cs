using System;

namespace Pixel_3_Toolkit.Models
{
    public class Images
    {
        public Images(string version, string versionCode)
        {
            Version = version ?? throw new ArgumentNullException(nameof(version));
            VersionCode = versionCode ?? throw new ArgumentNullException(nameof(versionCode));
        }

        public string Version { get; }
        public string VersionCode { get; }
    }
}

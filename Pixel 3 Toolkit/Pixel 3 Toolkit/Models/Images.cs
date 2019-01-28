using System;

namespace Pixel_3_Toolkit.Models
{
    public class Images
    {
        public Images(string version, int versionCode)
        {
            Version = version ?? throw new ArgumentNullException(nameof(version));
            VersionCode = versionCode;
        }

        public string Version { get; }
        public int VersionCode { get; }
    }
}

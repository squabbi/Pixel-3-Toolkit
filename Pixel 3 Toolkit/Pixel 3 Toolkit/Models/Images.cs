using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixel_3_Toolkit.Models
{
    class Images
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

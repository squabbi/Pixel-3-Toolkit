using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixel_3_Toolkit.Models
{
    public class Toolkit
    {
        public Toolkit(string version, int versionCode, string updateLink, string xdaThreadLink, string note)
        {
            Version = version ?? throw new ArgumentNullException(nameof(version));
            VersionCode = versionCode;
            UpdateLink = updateLink ?? throw new ArgumentNullException(nameof(updateLink));
            XdaThreadLink = xdaThreadLink ?? throw new ArgumentNullException(nameof(xdaThreadLink));
            Note = note ?? throw new ArgumentNullException(nameof(note));
        }

        public string Version { get; }
        public int VersionCode { get; }
        public string UpdateLink { get; }
        public string XdaThreadLink { get; }
        public string Note { get; }
    }
}

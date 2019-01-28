using System;

namespace Pixel_3_Toolkit.Models
{
    class FactoryImage
    {
        public FactoryImage(string codename, string androidVersion, string androidBuild, string build, string date, string link, string checksum)
        {
            this.Codename = codename ?? throw new ArgumentNullException(nameof(codename));
            this.AndroidVersion = androidVersion ?? throw new ArgumentNullException(nameof(androidVersion));
            this.AndroidBuild = androidBuild ?? throw new ArgumentNullException(nameof(androidBuild));
            this.Build = build ?? throw new ArgumentNullException(nameof(build));
            this.Date = date ?? throw new ArgumentNullException(nameof(date));
            this.Link = link ?? throw new ArgumentNullException(nameof(link));
            this.Checksum = checksum ?? throw new ArgumentNullException(nameof(checksum));
        }

        public string Codename { get; }

        public string AndroidVersion { get; }

        public string AndroidBuild { get; }

        public string Build { get; }

        public string Date { get; }

        public string Link { get; }

        public string Checksum { get; }

        public override string ToString()
        {
            return String.Format("Android {0}, {1} - {2}", AndroidVersion, Date, AndroidBuild);
        }
    }
}

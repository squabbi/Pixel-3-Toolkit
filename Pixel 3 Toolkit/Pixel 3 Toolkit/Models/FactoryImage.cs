using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixel_3_Toolkit.Models
{
    class FactoryImage
    {
        private readonly string codename;
        private readonly string androidVersion;
        private readonly string androidBuild;
        private readonly string build;
        private readonly string date;
        private readonly string link;
        private readonly string checksum;

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

        public string Codename
        {
            get { return codename; }
        }

        public string AndroidVersion
        {
            get { return androidVersion; }
        }

        public string AndroidBuild
        {
            get { return androidBuild; }
        }

        public string Build
        {
            get { return build; }
        }

        public string Date
        {
            get { return date; }
        }

        public string Link
        {
            get { return link; }
        }

        public string Checksum
        {
            get { return checksum; }
        }

        public override string ToString()
        {
            return String.Format("Android {0}, {1} - {2}", AndroidVersion, Date, AndroidBuild);
        }
    }
}

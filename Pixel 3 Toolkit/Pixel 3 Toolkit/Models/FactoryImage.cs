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
            this.codename = codename ?? throw new ArgumentNullException(nameof(codename));
            this.androidVersion = androidVersion ?? throw new ArgumentNullException(nameof(androidVersion));
            this.androidBuild = androidBuild ?? throw new ArgumentNullException(nameof(androidBuild));
            this.build = build ?? throw new ArgumentNullException(nameof(build));
            this.date = date ?? throw new ArgumentNullException(nameof(date));
            this.link = link ?? throw new ArgumentNullException(nameof(link));
            this.checksum = checksum ?? throw new ArgumentNullException(nameof(checksum));
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
            return String.Format("Android {0}, {1} - {2}", androidVersion, date, androidBuild);
        }
    }
}

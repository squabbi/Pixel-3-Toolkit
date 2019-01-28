using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixel_3_Toolkit.Models
{
    class Client
    {
        public Client(Toolkit toolkit, Images images)
        {
            Toolkit = toolkit ?? throw new ArgumentNullException(nameof(toolkit));
            Images = images ?? throw new ArgumentNullException(nameof(images));
        }

        public Toolkit Toolkit { get; }
        public Images Images { get; }

    }
}

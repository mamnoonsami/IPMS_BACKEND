using System;
using System.Collections.Generic;

#nullable disable

namespace PEI_API.EF
{
    public partial class PeiWeedimage
    {
        public int WiId { get; set; }
        public int WId { get; set; }
        public string WiPhotoUrl { get; set; }

        public virtual PeiWeed WIdNavigation { get; set; }
    }
}

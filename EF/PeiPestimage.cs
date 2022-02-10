using System;
using System.Collections.Generic;

#nullable disable

namespace PEI_API.EF
{
    public partial class PeiPestimage
    {
        public int PiId { get; set; }
        public int PId { get; set; }
        public string PiPhotoUrl { get; set; }

        public virtual PeiPest PIdNavigation { get; set; }
    }
}

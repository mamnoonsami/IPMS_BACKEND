using System;
using System.Collections.Generic;

#nullable disable

namespace PEI_API.EF
{
    public partial class PeiCropsdisease
    {
        public int CId { get; set; }
        public int DId { get; set; }

        public virtual PeiCrop CIdNavigation { get; set; }
        public virtual PeiDisease DIdNavigation { get; set; }
    }
}

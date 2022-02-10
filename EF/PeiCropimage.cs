using System;
using System.Collections.Generic;

#nullable disable

namespace PEI_API.EF
{
    public partial class PeiCropimage
    {
        public int CiId { get; set; }
        public int CId { get; set; }
        public string CiPhotoUrl { get; set; }

        public virtual PeiCrop CIdNavigation { get; set; }
    }
}

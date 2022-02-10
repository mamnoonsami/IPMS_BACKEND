using System;
using System.Collections.Generic;

#nullable disable

namespace PEI_API.EF
{
    public partial class PeiCropspest
    {
        public int PId { get; set; }
        public int CId { get; set; }

        public virtual PeiCrop CIdNavigation { get; set; }
        public virtual PeiPest PIdNavigation { get; set; }
    }
}

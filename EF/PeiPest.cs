using System;
using System.Collections.Generic;

#nullable disable

namespace PEI_API.EF
{
    public partial class PeiPest
    {
        public PeiPest()
        {
            PeiCoordinatesPests = new HashSet<PeiCoordinatesPest>();
            PeiCropspests = new HashSet<PeiCropspest>();
            PeiPestimages = new HashSet<PeiPestimage>();
        }

        public int PId { get; set; }
        public string PName { get; set; }
        public string PPhotoUrl { get; set; }
        public string PDescription { get; set; }

        public virtual PeiPestInfo PeiPestInfo { get; set; }
        public virtual ICollection<PeiCoordinatesPest> PeiCoordinatesPests { get; set; }
        public virtual ICollection<PeiCropspest> PeiCropspests { get; set; }
        public virtual ICollection<PeiPestimage> PeiPestimages { get; set; }
    }
}

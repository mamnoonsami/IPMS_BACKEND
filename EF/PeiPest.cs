using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        public IFormFile PImageFile { get; set; } // [NotMapped] is used so that it doesnot get added to the sql table
        [NotMapped]
        public string PImageSrc { get; set; } // not in data table

        public virtual PeiPestInfo PeiPestInfo { get; set; }
        public virtual ICollection<PeiCoordinatesPest> PeiCoordinatesPests { get; set; }
        public virtual ICollection<PeiCropspest> PeiCropspests { get; set; }
        public virtual ICollection<PeiPestimage> PeiPestimages { get; set; }
    }
}

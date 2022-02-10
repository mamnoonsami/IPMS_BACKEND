using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace PEI_API.EF
{
    public partial class PeiCrop
    {
        public PeiCrop()
        {
            PeiCropimages = new HashSet<PeiCropimage>();
            PeiCropsdiseases = new HashSet<PeiCropsdisease>();
            PeiCropspests = new HashSet<PeiCropspest>();
        }

        public int CId { get; set; }
        public string CName { get; set; }
        public string CPhotoUrl { get; set; }
        public string CDescription { get; set; }

        [NotMapped]
        public IFormFile CImageFile { get; set; } // [NotMapped] is used so that it doesnot get added to the sql table
        [NotMapped]
        public string CImageSrc { get; set; } // not in data table

        public virtual ICollection<PeiCropimage> PeiCropimages { get; set; }
        public virtual ICollection<PeiCropsdisease> PeiCropsdiseases { get; set; }
        public virtual ICollection<PeiCropspest> PeiCropspests { get; set; }
    }
}

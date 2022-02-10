using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace PEI_API.EF
{
    public partial class PeiFeedback
    {
        public PeiFeedback()
        {
            PeiReplies = new HashSet<PeiReply>();
        }

        public int FId { get; set; }
        public int UId { get; set; }
        public string UEmail { get; set; }
        public string FComment { get; set; }
        public string FImageName { get; set; }

        [NotMapped]
        public IFormFile FImageFile { get; set; } // [NotMapped] is used so that it doesnot get added to the sql table
        [NotMapped]
        public string FImageSrc { get; set;  } // not in data table
        public DateTime? FTimeStamp { get; set; }

        public virtual PeiUser U { get; set; }
        public virtual ICollection<PeiReply> PeiReplies { get; set; }
    }
}

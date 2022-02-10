using Microsoft.AspNetCore.Http;
using PEI_API.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PEI_API.Models
{
    public class FeedbacksModel
    {
        public int FId { get; set; }
        public int UId { get; set; }
        public string UEmail { get; set; }
        public string FComment { get; set; }
        public string FImageName { get; set; }
        public IFormFile ImageFile { get; set; }
        public DateTime? FTimeStamp { get; set; }

        public virtual PeiUser U { get; set; }
        public virtual ICollection<PeiReply> PeiReplies { get; set; }
    }
}

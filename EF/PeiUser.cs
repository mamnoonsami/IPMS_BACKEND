using System;
using System.Collections.Generic;

#nullable disable

namespace PEI_API.EF
{
    public partial class PeiUser
    {
        public PeiUser()
        {
            PeiFeedbacks = new HashSet<PeiFeedback>();
            PeiReplies = new HashSet<PeiReply>();
        }

        public int UId { get; set; }
        public string UFirstName { get; set; }
        public string ULastName { get; set; }
        public string UEmail { get; set; }
        public bool? UStatus { get; set; }
        public string UPassword { get; set; }
        public DateTime? UTimeStamp { get; set; }
        public string UAuthLevel { get; set; }
        public string UAccessToken { get; set; }

        public virtual ICollection<PeiFeedback> PeiFeedbacks { get; set; }
        public virtual ICollection<PeiReply> PeiReplies { get; set; }
    }
}

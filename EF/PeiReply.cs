using System;
using System.Collections.Generic;

#nullable disable

namespace PEI_API.EF
{
    public partial class PeiReply
    {
        public int RId { get; set; }
        public int FId { get; set; }
        public int UId { get; set; }
        public string UEmail { get; set; }
        public string RReply { get; set; }
        public DateTime? RTimeStamp { get; set; }

        public virtual PeiFeedback FIdNavigation { get; set; }
        public virtual PeiUser U { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace PEI_API.EF
{
    public partial class PeiDisease
    {
        public PeiDisease()
        {
            PeiCropsdiseases = new HashSet<PeiCropsdisease>();
        }

        public int DId { get; set; }
        public string DName { get; set; }
        public string DDescription { get; set; }

        public virtual ICollection<PeiCropsdisease> PeiCropsdiseases { get; set; }
    }
}

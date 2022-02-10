using System;
using System.Collections.Generic;

#nullable disable

namespace PEI_API.EF
{
    public partial class PeiWeed
    {
        public PeiWeed()
        {
            PeiWeedimages = new HashSet<PeiWeedimage>();
        }

        public int WId { get; set; }
        public string WName { get; set; }
        public string WPhotoUrl { get; set; }
        public string WDescription { get; set; }

        public virtual ICollection<PeiWeedimage> PeiWeedimages { get; set; }
    }
}

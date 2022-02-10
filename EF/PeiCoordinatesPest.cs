using System;
using System.Collections.Generic;

#nullable disable

namespace PEI_API.EF
{
    public partial class PeiCoordinatesPest
    {
        public int CoordId { get; set; }
        public int? PId { get; set; }
        public int? UId { get; set; }
        public decimal? CoordLat { get; set; }
        public decimal? CoordLng { get; set; }

        public virtual PeiPest PIdNavigation { get; set; }
    }
}

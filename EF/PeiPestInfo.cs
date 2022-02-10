using System;
using System.Collections.Generic;

#nullable disable

namespace PEI_API.EF
{
    public partial class PeiPestInfo
    {
        public int PId { get; set; }
        public string PinBiologicalinfo { get; set; }
        public string PinMonitoringMethod { get; set; }
        public string PinControlThreshold { get; set; }
        public string PinPhysicalControl { get; set; }
        public string PinBiologicalControl { get; set; }
        public string PinCulturalControl { get; set; }
        public string PinChemicalControl { get; set; }

        public virtual PeiPest PIdNavigation { get; set; }
    }
}

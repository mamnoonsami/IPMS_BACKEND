using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PEI_API.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PEI_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PestsController : ControllerBase
    {
        private peiDBContext _db;

        public PestsController(peiDBContext db)
        {
            _db = db;

        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            /*var crops = await _db.ICrops.ToListAsync();*/

            var pests = await (from pest in _db.PeiPests
                               orderby pest.PName
                               select new
                               {
                                   pest = pest.PName,
                                   image = pest.PPhotoUrl,
                                   description = pest.PDescription

                               }).ToListAsync();

            return Ok(pests);
        }

        [Route("Get/{id}")]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var pest = await (from p in _db.PeiPests
                              where p.PId == id
                              select new
                              {
                                  pest = p.PName,
                                  image = p.PPhotoUrl

                              }).ToListAsync();

            if (pest is null)
                return BadRequest("No data found");

            return Ok(pest);
        }

        [Route("GetDescription/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetDescription(int id)
        {

            var pestD = await _db.PeiPests
                        .Where(p => p.PId == id)
                        .Select(p => new
                        {
                            pestName = p.PName,
                            biologicalInfo = p.PeiPestInfo.PinBiologicalinfo,
                            monitoringMethod = p.PeiPestInfo.PinMonitoringMethod,
                            controlThreshold = p.PeiPestInfo.PinControlThreshold,
                            physicalControl = p.PeiPestInfo.PinPhysicalControl,
                            biologicalControl = p.PeiPestInfo.PinBiologicalControl,
                            culturalControl = p.PeiPestInfo.PinCulturalControl,
                            chemicalControl = p.PeiPestInfo.PinChemicalControl
                        }).ToListAsync();


            if (pestD is null)
                return BadRequest("No data found");

            return Ok(pestD);
        }

        [Route("updatedescription")]
        [HttpPut]
        public async Task<IActionResult> updatedescription(PeiPestInfo info)
        {
            if (string.IsNullOrEmpty((info.PId).ToString()))
            {
                return BadRequest("No Id specified");
            }

            var pestInfo = await _db.PeiPestInfos.FindAsync(info.PId);

            if(pestInfo is null)
            {
                return BadRequest("Incorrect ID");
            }

            pestInfo.PinBiologicalinfo = info.PinBiologicalinfo;
            pestInfo.PinMonitoringMethod = info.PinMonitoringMethod;
            pestInfo.PinControlThreshold = info.PinControlThreshold;
            pestInfo.PinBiologicalControl = info.PinBiologicalControl;
            pestInfo.PinChemicalControl = info.PinChemicalControl;
            pestInfo.PinPhysicalControl = info.PinPhysicalControl;
            pestInfo.PinCulturalControl = info.PinCulturalControl;

            _db.PeiPestInfos.Attach(pestInfo);
            await _db.SaveChangesAsync();

            return Ok(pestInfo);

        }
    }
}

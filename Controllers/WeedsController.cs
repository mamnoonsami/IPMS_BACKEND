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
    public class WeedsController : ControllerBase
    {
        private peiDBContext _db;

        public WeedsController(peiDBContext db)
        {
            _db = db;
        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            /*var crops = await _db.ICrops.ToListAsync();*/

            var weeds = await (from weed in _db.PeiWeeds
                               orderby weed.WName
                               select new
                               {
                                   id = weed.WId,
                                   crop = weed.WName,
                                   image = weed.WPhotoUrl,
                                   description = weed.WDescription

                               }).ToListAsync();

            return Ok(weeds);
        }
    }
}

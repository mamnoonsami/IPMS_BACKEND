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
    public class DiseasesController : ControllerBase
    {
        private peiDBContext _db;

        public DiseasesController(peiDBContext db)
        {
            _db = db;
        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var diseases = await (from disease in _db.PeiDiseases
                                  orderby disease.DName
                                  select new
                                  {
                                      disease = disease.DName

                                  }).ToListAsync();

            return Ok(diseases);
        }

        [Route("Get/{id}")]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var disease = await (from d in _db.PeiDiseases
                              where d.DId == id
                              select new
                              {
                                  disease = d.DName,

                              }).ToListAsync();

            if (disease is null)
                return BadRequest("No data found");

            return Ok(disease);
        }

    }
}

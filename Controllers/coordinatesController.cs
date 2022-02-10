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
    public class coordinatesController : ControllerBase
    {
        private peiDBContext _db;

        public coordinatesController(peiDBContext db)
        {
            _db = db;
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(PeiCoordinatesPest coord)
        {
            await _db.PeiCoordinatesPests.AddAsync(coord);
            await _db.SaveChangesAsync();

            return Ok();

        }
    }
}

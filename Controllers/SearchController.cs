using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PEI_API.EF;
using PEI_API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PEI_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private peiDBContext _db;
        private readonly JwtService _jwtService;

        public SearchController(peiDBContext db, JwtService jwtService)
        {
            _db = db;
            _jwtService = jwtService;

        }

        [Route("SearchQuery/{keyword}")]
        [HttpGet]
        public async Task<IActionResult> SearchQuery(string keyword)
        {
            var searchResult = await _db.PeiCrops
                                   .Where(k => keyword.Contains(k.CName) || k.CName.Contains(keyword))
                                  .Select(c => new {
                                      id = c.CId,
                                      crop = c.CName,
                                      image = c.CPhotoUrl,
                                  }).ToListAsync();
            if (!searchResult.Any())
            {
                return BadRequest(new { message ="Could not find anything"});
            }
            return Ok(searchResult);
        }
    }
}

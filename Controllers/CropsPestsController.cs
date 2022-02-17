using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PEI_API.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PEI_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CropsPestsController : ControllerBase
    {
        private peiDBContext _db;

        private readonly IWebHostEnvironment _hostEnvironment;

        public CropsPestsController(peiDBContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            this._hostEnvironment = hostEnvironment;
        }


        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]PeiCropspest cp)
        {
            var pId = _db.PeiPests.Find(cp.PId);
            if(pId is null)
            {
                return Ok(new { message = "Check" });
            }
            else
            {
                await _db.PeiCropspests.AddAsync(cp);
                await _db.SaveChangesAsync();

                return Ok(new { message = "Record has been added" });
            }
        }

        [Route("DeletePest")]
        [HttpDelete]
        public async Task<IActionResult> DeletePest(PeiCropspest cp)
        {
            /*var obj = _db.PeiCrops.Find(pestId);

            if(obj is null)
            {
                return BadRequest(new { message = "Nothing found " });
            }

            else
            {
                return Ok();
            }*/
            var obj =  _db.PeiCropspests.Where(c => (c.PId == cp.PId) && (c.CId == cp.CId));
            bool isEmpty = !obj.Any();

            //_db.PeiCropspests.Remove(cropId, pestId);
            //await _db.SaveChangesAsync();
            //return Ok(cp);

            if (obj is null)
            {
                return BadRequest("No data found");

            }

            else
            {
                _db.PeiCropspests.Remove(cp);
                await _db.SaveChangesAsync();
                return Ok(cp);
                
            }


        }

    }


}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class PestsController : ControllerBase
    {
        private peiDBContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PestsController(peiDBContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            this._hostEnvironment = hostEnvironment;
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
                                   pId = pest.PId,
                                   pest = pest.PName,
                                   //image = pest.PPhotoUrl,
                                   image = String.Format("{0}://{1}{2}/images/cropsPests/{3}", Request.Scheme, Request.Host, Request.PathBase, pest.PPhotoUrl),
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
                                  //image = p.PPhotoUrl
                                  image = String.Format("{0}://{1}{2}/images/cropsPests/{3}", Request.Scheme, Request.Host, Request.PathBase, p.PPhotoUrl),

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

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PeiPest pest)
        {
            pest.PPhotoUrl = await SaveImage(pest.PImageFile);

            var findPest = await _db.PeiPests.Where(p => p.PName == pest.PName).ToListAsync();
            bool isEmpty = !findPest.Any();

            if (isEmpty)
            {
                await _db.PeiPests.AddAsync(pest);
                await _db.SaveChangesAsync();

                return Ok(new { message = "Record has been added" });
            }

            else
            {
                return Ok(new { message = "Conflict" });
            }

        }

        [Route("DeletePest/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePest(int id)
        {

            var pestId = _db.PeiPests.Find(id);
            if (pestId is null)
            {
                return BadRequest("No data found");
            }

            else
            {
                DeleteImage(pestId.PPhotoUrl);
                _db.PeiPests.Remove(pestId);
                await _db.SaveChangesAsync();
                return Ok(pestId);
            }


        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = "";
            if (imageFile != null)
            {
                imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
                var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "images/cropsPests", imageName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
            }

            return imageName;
        }


        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "images/cropsPests", imageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }
    }
}

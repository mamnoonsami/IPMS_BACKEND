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
    public class CropsController : ControllerBase
    {
        private peiDBContext _db;

        private readonly IWebHostEnvironment _hostEnvironment;

        public CropsController(peiDBContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            this._hostEnvironment = hostEnvironment;
        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            /*var crops = await _db.ICrops.ToListAsync();*/

            var crops = await (from crop in _db.PeiCrops
                               orderby crop.CName
                               select new
                               {
                                   id = crop.CId,
                                   crop = crop.CName,
                                   //image = crop.CPhotoUrl,
                                   image = String.Format("{0}://{1}{2}/images/crops/{3}", Request.Scheme, Request.Host, Request.PathBase, crop.CPhotoUrl),
                                   description = crop.CDescription

                               }).ToListAsync();

            return Ok(crops);
        }

        [Route("Get/{id}")]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var crop = await (from c in _db.PeiCrops
                              where c.CId == id
                               select new
                               {
                                   crop = c.CName,
                                   image = c.CPhotoUrl

                               }).ToListAsync();

            if (crop is null)
                return BadRequest("No data found");

            return Ok(crop);
        }

        [Route("Getspecific/{cropId}")]
        [HttpGet]
        public async Task<IActionResult> GetSpecific(int cropId)
        {
            var cropDetails = await _db.PeiCrops
                               .Where(c => c.CId == cropId)
                               .Select(c => new {
                                   CropId = c.CId,
                                   CropName = c.CName,
                                   PestDetails = c.PeiCropspests.Select(p => new {
                                       PId = p.PIdNavigation.PId,
                                       PName = p.PIdNavigation.PName,
                                       PUrl = p.PIdNavigation.PPhotoUrl
                                   })
                               }).ToListAsync();
            /*var crops = await _db.ICrops.ToListAsync();*/

            //var cropDetails = await _db.PeiCrops.Where(c=>c.CId == cropId).Include(i=>i.PeiCropspests).ToListAsync();
            /* var cropDetails = await _db.PeiCrops
                             .Where(c => c.CId == cropId)
                             .Include(i => i.PeiCropspests)
                             .ThenInclude(t => t.PIdNavigation)
                             .Select(s => new { 
                                                 CropId = s.CId, 
                                                 PestDetails = s.PeiCropspests
                                                 .Select( a => new { 
                                                                     PId = a.PId, 
                                                                     PName = a.PIdNavigation.PName,
                                                                     PUrl = a.PIdNavigation.PPhotoUrl
                                                                    }
                                                         )
                                             }
                                     )
                             .ToListAsync();*/

            /*var cropDetails = await _db.PeiCrops
                            .Where(c => c.CId == cropId)
                            .Include(i => i.PeiCropspests)
                            .ThenInclude(t => t.PIdNavigation)
                            .Select(s => new { CropId = s.CId, PestName = s.PeiCropspests })
                            .ToListAsync();*/


            return Ok(cropDetails);
        }
        [Route("GetDiseases/{cropId}")]
        [HttpGet]
        public async Task<IActionResult> GetDiseases(int cropId)
        {
            var cropDetails = await _db.PeiCrops
                               .Where(c => c.CId == cropId)
                               .Select(c => new {
                                   CropId = c.CId,
                                   CropName = c.CName,
                                   diseases = c.PeiCropsdiseases.Select(d => new {
                                       DId = d.DIdNavigation.DId,
                                       DName = d.DIdNavigation.DName,
                                   })
                               }).ToListAsync();

            return Ok(cropDetails);
        }


        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]PeiCrop crop)
        {
            crop.CPhotoUrl = await SaveImage(crop.CImageFile);

            var findCrop = await _db.PeiCrops
                               .Where(c => c.CName == crop.CName).ToListAsync();
            bool isEmpty = !findCrop.Any();

            if (isEmpty)
            {
                await _db.PeiCrops.AddAsync(crop);
                await _db.SaveChangesAsync();

                return Ok(new { message = "Record has been added"});
            }

            else
            {
                return Ok(new { message = "Conflict" });
            }

        }
        [Route("DeleteCrop/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCrop(int id)
        {

            var cropId = _db.PeiCrops.Find(id);
            if (cropId is null)
            {
                return BadRequest("No data found");
            }

            else
            {
                DeleteImage(cropId.CPhotoUrl);
                _db.PeiCrops.Remove(cropId);
                await _db.SaveChangesAsync();
                return Ok(cropId);
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
                var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "images/crops", imageName);
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
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "images/crops", imageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }
    }
}

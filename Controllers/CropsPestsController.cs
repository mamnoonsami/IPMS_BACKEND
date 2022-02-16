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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PEI_API.EF;
using System;
using System.Management;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using PEI_API.Helpers;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace PEI_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private peiDBContext _db;
        private readonly JwtService _jwtService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CommentsController(peiDBContext db, JwtService jwtService, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _jwtService = jwtService;
            this._hostEnvironment = hostEnvironment;
        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

           /* var comments = await _db.PeiFeedbacks
                                  .OrderByDescending(com => com.FTimeStamp)
                                  .Select(c => new {
                                      FId = c.FId,
                                      uName = c.U.UFirstName,
                                      comment = c.FComment,
                                      time = c.FTimeStamp,
                                       replies = c.PeiReplies
                                      .OrderBy(rep => rep.RTimeStamp)
                                      .Select(r => new
                                      {
                                          uName = r.U.UFirstName,
                                          reply = r.RReply,
                                          time = r.RTimeStamp
                                      })

                                  }).ToListAsync();*/

            var comments = await _db.PeiFeedbacks
                                 .OrderByDescending(com => com.FTimeStamp)
                                 .Select(c => new {
                                     FId = c.FId,
                                     uName = c.U.UFirstName,
                                     comment = c.FComment,
                                     imageName = c.FImageName,
                                     imageSrc = String.Format("{0}://{1}{2}/images/comments/{3}", Request.Scheme, Request.Host, Request.PathBase, c.FImageName),
                                     time = c.FTimeStamp,
                                 }).ToListAsync();
            return Ok(comments);
        }
        [Route("GetReplies/{commentId}")]
        [HttpGet]
        public async Task<IActionResult> GetReplies(int commentId)
        {

            var replies = await _db.PeiReplies
                                  .Where(r => r.FId == commentId)
                                  .OrderBy(rep => rep.RTimeStamp)
                                  .Select(re => new
                                  {
                                      replyId = re.RId,
                                      uName = re.U.UFirstName,
                                      reply = re.RReply,
                                      time = re.RTimeStamp
                                  }).ToListAsync();

            return Ok(replies);
        }
        [Route("PostComment")]
        [HttpPost]
        public async Task<IActionResult> PostComment([FromForm]PeiFeedback comment)
        {
            comment.FImageName = await SaveImage(comment.FImageFile);
            _db.PeiFeedbacks.Add(comment);
            await _db.SaveChangesAsync();

            return Ok();

        }

        [Route("PostReply")]
        [HttpPost]
        public async Task<IActionResult> PostReply(PeiReply reply)
        {
            await _db.PeiReplies.AddAsync(reply);
            await _db.SaveChangesAsync();

            return Ok();

        }


        [Route("DeleteComment/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);

                IEnumerable<String> aud = token.Audiences;
                int userId = int.Parse(token.Issuer);


                if (aud.Contains("Admin")) // admin's user id is 27
                {
                    var comment = _db.PeiFeedbacks.Find(id);

                    if (comment is null)
                    {
                        return BadRequest("No data found");
                    }
                    else
                    {
                        DeleteImage(comment.FImageName);
                        _db.PeiFeedbacks.Remove(comment);
                        _db.SaveChangesAsync();
                        return Ok(comment);
                    }
                }
                else
                {
                    return Unauthorized(new { message = "You are not authorized" });
                }

                
            }
            catch (Exception)
            {
                return Unauthorized(new { message = "You are not authorized" });
            }

        }


        [Route("DeleteReply/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteReply(int id)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);

                int userId = int.Parse(token.Issuer);
                IEnumerable<String> aud = token.Audiences;
                //Console.WriteLine(aud);
                

                if (aud.Contains("Admin")) // admin's user id is 27
                {
                    var reply = _db.PeiReplies.Find(id);

                    if (reply is null)
                    {
                        return BadRequest("No data found");
                    }
                    else
                    {
                        _db.PeiReplies.Remove(reply);
                        _db.SaveChangesAsync();
                        return Ok(reply);
                    }
                }
                else
                {
                    return Unauthorized(new { message = "You are not authorized" });
                }


            }
            catch (Exception)
            {
                return Unauthorized(new { message = "You are not authorized" });
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
                var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "images/comments", imageName);
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
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "images/comments", imageName);
            if(System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

    }
}

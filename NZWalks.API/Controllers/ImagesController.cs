using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

        //POST : /api/Images/Upload
        [HttpPost]
        [Route("Upload")]

        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request )
        {
            ValidateFileUpload(request);

            if (ModelState.IsValid)
            {
                //User Repository to Upload Image

            }

            return BadRequest(ModelState);

        }


        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] {".jpg", ".jpeg",".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)) )
                {
                  ModelState.AddModelError("file","File Not Suppported");
                }
            if (request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File Size more than 10MB, please upload a smaller size file.");
            }

        }
    }
}

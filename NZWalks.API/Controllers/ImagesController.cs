using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }



        //POST : /api/Images/Upload
        [HttpPost]
        [Route("Upload")]

        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request )
        {
            ValidateFileUpload(request);

            if (ModelState.IsValid)
            {
                //Convert DTO to Domain Model

                var ImageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.File.FileName,
                    FileDescription = request.FileDescription
                };

                //User Repository to Upload Image

                await imageRepository.Upload(ImageDomainModel);
                return Ok(ImageDomainModel);
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

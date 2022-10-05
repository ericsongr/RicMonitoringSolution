using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RicEntityFramework.Interfaces;
using RicMonitoringAPI.Common.Model;
using RicMonitoringAPI.RoomRent.ViewModels;

namespace RicMonitoringAPI.RoomRent.Controllers
{
    [Authorize(Policy = "SuperAndAdmin")]
    //[AllowAnonymous]
    [Route("api/renter-file-upload")]
    [ApiController]
    public class RentersFileUploadController : ApiBaseController
    {
        private readonly IImageService _imageService;

        public RentersFileUploadController(IImageService imageService)
        {
            _imageService = imageService ?? throw new Exception(nameof(imageService));
        }
        [HttpPost()]
        public IActionResult ProfileImageUpload([FromBody] UploadImageBase64 model)
        {
            try
            {
                if (model.Base64.Contains("base64,"))
                {
                    var arr = model.Base64.Split(",");
                    model.Base64 = arr[1];
                }

                _imageService.Upload(model.RenterId, model.Base64);

                return Ok(new BaseRestApiModel
                {
                    Payload = "UPLOAD_COMPLETED",
                    Errors = new List<BaseError>(),
                    StatusCode = (int)HttpStatusCode.OK
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
            
        }


        ////still have error so i use base64
        //[HttpPost("web"), RequestFormLimits(KeyLengthLimit = int.MaxValue)]
        //public IActionResult WebProfileImageUpload(List<IFormFile> file)
        //{

        //    try
        //    {
        //        var file1 = file[0];
        //        var folderName = Path.Combine("Resources", "Images", "Profile");
        //        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //        if (file1.Length > 0)
        //        {
        //            var fileName = ContentDispositionHeaderValue.Parse(file1.ContentDisposition).FileName.Trim('"');
        //            var fullPath = Path.Combine(pathToSave, fileName);
        //            var dbPath = Path.Combine(folderName, fileName);
        //            using (var stream = new FileStream(fullPath, FileMode.Create))
        //            {
        //                file1.CopyTo(stream);
        //            }
        //            return Ok(new { dbPath });
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex}");
        //    }
        //}

        //[HttpPost("web"), DisableRequestSizeLimit]
        //public async Task<IActionResult> WebProfileImageUpload()
        //{

        //    try
        //    {
        //        var formCollection = await Request.ReadFormAsync();
        //        var file = formCollection.Files.First();
        //        var folderName = Path.Combine("Resources", "Images", "Profile");
        //        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //        if (file.Length > 0)
        //        {
        //            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //            var fullPath = Path.Combine(pathToSave, fileName);
        //            var dbPath = Path.Combine(folderName, fileName);
        //            using (var stream = new FileStream(fullPath, FileMode.Create))
        //            {
        //                file.CopyTo(stream);
        //            }
        //            return Ok(new { dbPath });
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex}");
        //    }
        //}
    }
}

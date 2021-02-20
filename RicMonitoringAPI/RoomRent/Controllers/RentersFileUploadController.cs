using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RicMonitoringAPI.RoomRent.ViewModels;

namespace RicMonitoringAPI.RoomRent.Controllers
{
    [Authorize(Policy = "SuperAndAdmin")]
    //[AllowAnonymous]
    [Route("api/renter-file-upload")]
    [ApiController]
    public class RentersFileUploadController : ApiBaseController
    {

        [HttpPost()]
        public Task<IActionResult> ProfileImageUpload([FromBody] UploadImageBase64 model)
        {
            var folderName = Path.Combine("Resources", "Images", "Profile");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            var fileName = $"{model.RenterId}.png";
            var imageBytes = Convert.FromBase64String(model.Base64);
            var fullPath = Path.Combine(pathToSave, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create)) 
            using (var ms = new MemoryStream(imageBytes))
            {
                ms.CopyTo(stream);
            }

            return null;
        }

        //[HttpPost, DisableRequestSizeLimit]
        //public async Task<IActionResult> ProfileImageUpload()
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

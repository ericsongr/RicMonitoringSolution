using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RicCommon.Constants;
using RicEntityFramework.Interfaces;
using RicMonitoringAPI.Common.Model;
using RicMonitoringAPI.RoomRent.Controllers;
using RicMonitoringAPI.ToolsInventory.ViewModels;

namespace RicMonitoringAPI.ToolsInventory.Controllers
{
    [Authorize(Policy = "SuperAndAdmin")]
    //[AllowAnonymous]
    [Route("api/tool-file-upload")]
    [ApiController]
    public class ToolFileUploadController : ApiBaseController
    {
        private readonly IImageService _imageService;

        public ToolFileUploadController(IImageService imageService)
        {
            _imageService = imageService ?? throw new Exception(nameof(imageService));
        }

        [HttpPost()]
        public IActionResult ImageUpload([FromBody] UploadToolImageBase64 model)
        {
            try
            {
                if (model.Base64.Contains("base64,"))
                {
                    var arr = model.Base64.Split(",");
                    model.Base64 = arr[1];
                }

                var filename = _imageService.Upload(model.Base64, FolderConstant.InventoryToolsImage);

                return Ok(new BaseRestApiModel
                {
                    Payload = new
                    {
                        status = "UPLOAD_COMPLETED",
                        filename = filename
                    },
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
        
    }
}

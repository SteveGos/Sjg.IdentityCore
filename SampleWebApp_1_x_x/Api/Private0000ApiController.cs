using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sjg.IdentityCore.Attributes;
using System;

namespace SampleWebApp_1_x_x.Api
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    [ApiBasicAuthorize]
    public class Private0000ApiController : ControllerBase
    {
        public Private0000ApiController()
        {
        }

        /// <summary>
        /// Post - Start Batch Processing.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<string> Post()
        {
            try
            {
                return $"API {nameof(Private0000ApiController)} {DateTime.Now} - No Roles Needed";
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
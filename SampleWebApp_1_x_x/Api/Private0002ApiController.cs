using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sjg.IdentityCore.Attributes;
using System;

namespace SampleWebApp_1_x_x.Api
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    [ApiBasicAuthorize(Roles = AccAuthRoles.Role2Name)]
    public class Private0002ApiController : ControllerBase
    {
        public Private0002ApiController()
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
                return $"API {nameof(Private0002ApiController)} {DateTime.Now} - {AccAuthRoles.Role2Name}";
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sjg.IdentityCore.Areas.UserMgmt;
using Sjg.IdentityCore.Attributes;
using System;

namespace SampleWebApp_1_x_x.Api
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    [ApiBasicAuthorize(Roles = AccAuthRoles.Role1Name)]
    public class Private0001ApiController : ControllerBase
    {
        public Private0001ApiController()
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
                return $"API {nameof(Private0001ApiController)} {DateTime.Now} - {AccAuthRoles.Role1Name}";
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
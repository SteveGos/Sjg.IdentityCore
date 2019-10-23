using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace SampleWebApp_1_x_x.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicApiController : ControllerBase
    {
        public PublicApiController()
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
                return $"API {nameof(PublicApiController)} {DateTime.Now}";
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
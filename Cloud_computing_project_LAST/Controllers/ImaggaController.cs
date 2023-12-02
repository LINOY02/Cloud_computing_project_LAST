using Cloud_computing_project_LAST.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Cloud_computing_project_LAST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImaggaController : ControllerBase
    {
        [HttpGet("CheckImage")]
        public async Task<IActionResult> CheckImage([FromQuery] string imageUrl)
        {
            try
            {
                ImaggaService imaggaService = new ImaggaService();

                string response = await imaggaService.CheckImage(imageUrl);
                string Okay = "Okay, the image contains coffee";

                if (response == Okay)
                {
                    return Ok(Okay);
                }
                else
                {
                    return Ok("Problem, this is not an image of coffee");
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}

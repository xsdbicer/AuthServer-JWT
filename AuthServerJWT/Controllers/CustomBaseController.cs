using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;

namespace AuthServerJWT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        public IActionResult ActionResultInstances<TInstance>(Response<TInstance> instance) where TInstance : class
        {
            return new ObjectResult(instance)
            {
                StatusCode = instance.StatusCode
            };
        }
    }
}

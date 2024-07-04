using Microsoft.AspNetCore.Mvc;

namespace EnsekCodingExercise.ApiService.Infrastructure.BaseControllers
{
    // I like to use a base controller to define common attributes for all controllers.
    // This way, I can avoid repeating the same attributes in every controller.
    // This is where I would put common functionality if needed.
    // For now, it's just an empty class.
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [Consumes("application/json", "application/xml")]
    [Produces("application/json", "application/xml")]
    public abstract class BaseController : ControllerBase
    {
    }
}

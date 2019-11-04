using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductsApp.Logic.Services.Interfaces;

namespace ProductsApp.WebApi.Controllers
{
#if DEBUG
    [Route("api/[controller]")]
    [ApiController]
    public class DebugController : ControllerBase
    {
        private readonly IDatabaseRestoreService _databaseRestoreService;

        public DebugController(IDatabaseRestoreService databaseRestoreService)
        {
            _databaseRestoreService = databaseRestoreService;
        }

        [HttpPost("resettestdata")]
        public async Task<IActionResult> ResetTestData()
        {
            var result = await _databaseRestoreService.Restore();

            if (!result.Success)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
#endif
}
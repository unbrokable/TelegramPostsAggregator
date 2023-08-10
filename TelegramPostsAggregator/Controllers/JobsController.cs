using Microsoft.AspNetCore.Mvc;
using TelegramPostAggregator.Services.Tasks;

namespace TelegramPostAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly TelegramPostsImporter telegramPostsImporter;

        public JobsController(TelegramPostsImporter telegramPostsImporter)
        {
            this.telegramPostsImporter = telegramPostsImporter;
        }

        [HttpPost("import/telegramposts")]
        public async Task<IActionResult> ImportTelegramPosts()
        {
            await telegramPostsImporter.Import();
            
            return Ok();
        }
            
    }
}

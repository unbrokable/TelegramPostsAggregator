using Microsoft.AspNetCore.Mvc;
using WTelegram;

namespace TelegramPostAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelegramController : ControllerBase
    {
        private readonly Client telegramClient;

        public TelegramController(Client telegramClient)
        {
            this.telegramClient = telegramClient;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string loginInfo)
        {
            string nextLoginInfo = await telegramClient.Login(loginInfo);

            return Ok(new
            {
                nextLoginInfo,
                isLogged = telegramClient.User is not null,
            });
        }

        [HttpGet("islogged")]
        public IActionResult IsLogged()
        {
            return Ok(new
            {
                isLogged = telegramClient.User is not null
            });
        }

        //[HttpPost("logout")]
        //public IActionResult Logout()
        //{
        //    telegramClient
        //}
    }
}

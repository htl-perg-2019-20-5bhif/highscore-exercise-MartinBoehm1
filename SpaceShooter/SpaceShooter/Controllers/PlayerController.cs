using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SpaceShooter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {

        private readonly ILogger<PlayerController> _logger;

        public PlayerController(ILogger<PlayerController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [Route("{index}")]
        public ActionResult<Player> Get(string index)
        {
            //Console.WriteLine("asdf\n\n\n\n\n\n\nasdf"+Program.Players.Count);
            return Ok(Program.Players[Int32.Parse(index)]);
        }
        [HttpGet]
        public ActionResult<Player[]> Get()
        {
            //Console.WriteLine("asdf\n\n\n\n\n\n\nasdf"+Program.Players.Count);
            return Ok(Program.Players.ToArray());
        }
        [HttpPost]
        [Route("{name}/{score}")]
        public ActionResult Post(string name, string score)
        {
            Program.Players.Add(new Player(name, Int32.Parse(score)));
            if (Program.Players.Count > 10)
            {
                Program.Players.Sort(new PlayerComparer().comparer);
                Program.Players.Remove(Program.Players[Program.Players.Count-1]);
            }
            return Ok();
        }
    }
}

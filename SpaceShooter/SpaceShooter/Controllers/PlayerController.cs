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
        PlayerContext context;

        public PlayerController(PlayerContext context)
        {
            this.context = context;
        }
        [HttpGet]
        [Route("{index}")]
        public ActionResult<Player> Get(string index)
        {
            //Console.WriteLine("asdf\n\n\n\n\n\n\nasdf"+Program.Players.Count);
            return Ok(context.Players.Where(p => p.PlayerId == Int32.Parse(index)));
        }
        [HttpGet]
        public ActionResult<Player[]> Get()
        {
            //Console.WriteLine("asdf\n\n\n\n\n\n\nasdf"+Program.Players.Count);
            return Ok(context.Players.ToArray());
        }
        [HttpDelete]
        public ActionResult WipeStats()
        {
            foreach (Player p in context.Players)
            {
                context.Players.Remove(p);
                context.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Route("{name}/{score}")]
        public ActionResult Post(string name, string score)
        {
            try
            {
                context.Players.Add(new Player()
                {
                    Name = name,
                    Score = Int32.Parse(score)
                });
                context.SaveChanges();
                while (context.Players.Count() > 10)
                {
                    int min = Int32.MaxValue;
                    int idMin = -1;
                    foreach (Player p in context.Players)
                    {
                        if (p.Score < min)
                        {
                            min = p.Score;
                            idMin = p.PlayerId;
                        }
                    }
                    context.Players.Remove(context.Players.Where(p => p.PlayerId == idMin).FirstOrDefault());
                    context.SaveChanges();
                }
                context.SaveChanges();

                //JsonConvert.SerializeObject(Program.Players);
                //System.IO.File.WriteAllText(@"WriteText.txt", JsonConvert.SerializeObject(Program.Players));

                return Ok();
            }catch(Exception e)
            {
                return BadRequest();
            }
            
        }
    }
}

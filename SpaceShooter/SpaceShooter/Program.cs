using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SpaceShooter
{
    public class Program
    {

        public static List<Player> Players { get; set; }
        public static void Main(string[] args)
        {
            Players = new List<Player>();
            Players.Add(new Player("p1", 1));
            Players.Add(new Player("p2", 2));
            Players.Add(new Player("p3", 3));
            Players.Add(new Player("p4", 4));
            Players.Add(new Player("p5", 5));
            Players.Add(new Player("p6", 6));
            Players.Add(new Player("p7", 7));
            Players.Add(new Player("p8", 8));
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

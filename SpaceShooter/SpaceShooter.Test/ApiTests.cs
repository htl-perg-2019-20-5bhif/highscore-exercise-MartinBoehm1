using SpaceShooter;
using SpaceShooter.Controllers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace SpaceShooter.Tests
{
    public class ApiTests
    {
        static async Task<HttpResponseMessage> Post(string name, string score)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(
                "http://localhost:5000/api/player/" + name+"/"+score,null);
            return response;
        }

        static async Task<HttpResponseMessage> Get()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:5000/api/player");
            return response;
        }
        [Fact]
        public void TestGetStatusCode()
        {
            string[] args = {};
            ThreadStart childref = new ThreadStart(SpaceShooter.Program.CreateHostBuilder(args).Build().Run);
            Thread childThread = new Thread(childref);
            childThread.Start();
            Thread.Sleep(5000);
            HttpResponseMessage response = Get().Result;
            Assert.True(response.IsSuccessStatusCode);
            childThread.Interrupt();
        }
        [Fact]
        public void TestGetPlayerArray()
        {
            string[] args = { };
            ThreadStart childref = new ThreadStart(SpaceShooter.Program.CreateHostBuilder(args).Build().Run);
            Thread childThread = new Thread(childref);
            childThread.Start();
            Thread.Sleep(5000);
            HttpResponseMessage response = Get().Result;
            bool b;

            try
            {
                JsonConvert.DeserializeObject<Player[]>(response.Content.ReadAsStringAsync().Result);
                b= true;
            } catch (Exception e) {
                b= false;
            }
            Assert.True(b);
            childThread.Interrupt();
        }

        [Fact]
        public void TestPostPlayer()
        {
            string[] args = { };
            ThreadStart childref = new ThreadStart(SpaceShooter.Program.CreateHostBuilder(args).Build().Run);
            Thread childThread = new Thread(childref);
            childThread.Start();
            Thread.Sleep(5000);
            HttpResponseMessage response = Post("name", "0").Result;
            Assert.True(response.IsSuccessStatusCode);
            childThread.Interrupt();
        }
        [Fact]
        public void TestPostNrPlayersRaises()
        {
            string[] args = { };
            ThreadStart childref = new ThreadStart(SpaceShooter.Program.CreateHostBuilder(args).Build().Run);
            Thread childThread = new Thread(childref);
            childThread.Start();
            Thread.Sleep(5000);
            HttpResponseMessage response = Get().Result;
            int lengthBefore=JsonConvert.DeserializeObject<Player[]>(response.Content.ReadAsStringAsync().Result).Length;
            response = Post("name", "0").Result;
            Assert.True(response.IsSuccessStatusCode);
            response = Get().Result;
            int lengthAfter = JsonConvert.DeserializeObject<Player[]>(response.Content.ReadAsStringAsync().Result).Length;
            Assert.True((lengthBefore == 9 && lengthAfter == 9) || (lengthBefore + 1 == lengthAfter));
            childThread.Interrupt();
        }
        [Fact]
        public void TestPostNrPlayersStopsRaising()
        {
            string[] args = { };
            ThreadStart childref = new ThreadStart(SpaceShooter.Program.CreateHostBuilder(args).Build().Run);
            Thread childThread = new Thread(childref);
            childThread.Start();
            Thread.Sleep(5000);
            HttpResponseMessage response = Get().Result;
            int newLength;
            int length = JsonConvert.DeserializeObject<Player[]>(response.Content.ReadAsStringAsync().Result).Length;
            while (length<10)
            {
                response = Post("name", "0").Result;
                Assert.True(response.IsSuccessStatusCode);
                response = Get().Result;
                newLength = JsonConvert.DeserializeObject<Player[]>(response.Content.ReadAsStringAsync().Result).Length;
                Assert.Equal(newLength, length + 1);
                length = newLength;
            }
            response = Post("name", "0").Result;
            Assert.True(response.IsSuccessStatusCode);
            response = Get().Result;
            newLength = JsonConvert.DeserializeObject<Player[]>(response.Content.ReadAsStringAsync().Result).Length;
            Assert.Equal(newLength, length);
            childThread.Interrupt();
        }
        /*[Fact]
        public void SetStoneInFullColumn()
        {
            var b = new GameBoard();
            for(var i = 0; i < 6; i++)
            {
                b.SetStone(0);
            }
            var previousPlayer = b.playerOne;
            Assert.Throws<InvalidOperationException>(() => b.SetStone(0));
            Assert.Equal(previousPlayer, b.playerOne);
        }
        [Fact]
        public void VerticalWin()
        {
            var b = new GameBoard();
            for (var i = 0; i < 3; i++)
            {
                b.SetStone(0);
                b.SetStone(1);

            }
            var result=b.SetStone(0);
            Assert.Equal(1, result);
        }
        [Fact]
        public void HorizontalWin()
        {
            var b = new GameBoard();
            for (byte i = 0; i < 3; i++)
            {
                Assert.Equal(0, b.SetStone(i));
                Assert.Equal(0, b.SetStone(i));

            }
            var result = b.SetStone(3);
            Assert.Equal(1, result);
        }
        [Fact]
        public void DiagonalWinLLTUR()
        {
            var b = new GameBoard();
            Assert.Equal(0, b.SetStone(0));//1
            Assert.Equal(0, b.SetStone(1));//2
            Assert.Equal(0, b.SetStone(1));//1
            Assert.Equal(0, b.SetStone(2));//2
            Assert.Equal(0, b.SetStone(2));//1
            Assert.Equal(0, b.SetStone(3));//2
            Assert.Equal(0, b.SetStone(2));//1
            Assert.Equal(0, b.SetStone(3));//2
            Assert.Equal(0, b.SetStone(3));//1
            Assert.Equal(0, b.SetStone(5));//2
            //Assert.Equal(0, b.SetStone(3));//1
            var result = b.SetStone(3);//1
            Assert.Equal(1, result);
        }
        [Fact]
        public void DiagonalWinLRTUL()
        {
            var b = new GameBoard();
            Assert.Equal(0, b.SetStone(3));//1
            Assert.Equal(0, b.SetStone(2));//2
            Assert.Equal(0, b.SetStone(2));//1
            Assert.Equal(0, b.SetStone(1));//2
            Assert.Equal(0, b.SetStone(1));//1
            Assert.Equal(0, b.SetStone(0));//2
            Assert.Equal(0, b.SetStone(1));//1
            Assert.Equal(0, b.SetStone(0));//2
            Assert.Equal(0, b.SetStone(0));//1
            Assert.Equal(0, b.SetStone(6));//2
            //Assert.Equal(0, b.SetStone(0));//1
            var result = b.SetStone(0);//1
            Assert.Equal(1, result);
        }
        [Fact]
        public void Draw()
        {
            var b = new GameBoard();
            Assert.Equal(0, b.SetStone(0));
            Assert.Equal(0, b.SetStone(1));
            Assert.Equal(0, b.SetStone(0));
            Assert.Equal(0, b.SetStone(1));
            Assert.Equal(0, b.SetStone(0));
            Assert.Equal(0, b.SetStone(1));
            Assert.Equal(0, b.SetStone(2));
            Assert.Equal(0, b.SetStone(3));
            Assert.Equal(0, b.SetStone(2));
            Assert.Equal(0, b.SetStone(3));
            Assert.Equal(0, b.SetStone(2));
            Assert.Equal(0, b.SetStone(3));
            Assert.Equal(0, b.SetStone(6));
            Assert.Equal(0, b.SetStone(0));
            Assert.Equal(0, b.SetStone(1));
            Assert.Equal(0, b.SetStone(0));
            Assert.Equal(0, b.SetStone(1));
            Assert.Equal(0, b.SetStone(0));
            Assert.Equal(0, b.SetStone(1));
            Assert.Equal(0, b.SetStone(2));
            Assert.Equal(0, b.SetStone(3));
            Assert.Equal(0, b.SetStone(2));
            Assert.Equal(0, b.SetStone(3));
            Assert.Equal(0, b.SetStone(2));
            Assert.Equal(0, b.SetStone(3));
            Assert.Equal(0, b.SetStone(5));
            Assert.Equal(0, b.SetStone(4));
            Assert.Equal(0, b.SetStone(5));
            Assert.Equal(0, b.SetStone(4));
            Assert.Equal(0, b.SetStone(5));
            Assert.Equal(0, b.SetStone(4));
            Assert.Equal(0, b.SetStone(4));
            Assert.Equal(0, b.SetStone(5));
            Assert.Equal(0, b.SetStone(4));
            Assert.Equal(0, b.SetStone(5));
            Assert.Equal(0, b.SetStone(4));
            Assert.Equal(0, b.SetStone(5));
            Assert.Equal(0, b.SetStone(6));
            Assert.Equal(0, b.SetStone(6));
            Assert.Equal(0, b.SetStone(6));
            Assert.Equal(0, b.SetStone(6));
            var result = b.SetStone(6);//1
            Assert.Equal(3, result);
        }*/
    }
}

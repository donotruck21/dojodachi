using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using dojodachi.Models;
using Microsoft.AspNetCore.Http;

namespace dojodachi.Controllers
{
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
    public class HomeController : Controller
    {
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetObjectFromJson<Dojodachi>("my_dachi") == null){
                HttpContext.Session.SetObjectAsJson("my_dachi", new Dojodachi());
            }

            if(HttpContext.Session.GetString("action") != null){
                ViewBag.picture = HttpContext.Session.GetString("action");
            }

            if(HttpContext.Session.GetString("log") != null){
                ViewBag.log = HttpContext.Session.GetString("log");
            }

            ViewBag.my_dachi = HttpContext.Session.GetObjectFromJson<Dojodachi>("my_dachi");
            return View();
        }



        // GET: /feed/
        [HttpGet]
        [Route("feed")]
        public IActionResult Feed()
        {
            System.Console.WriteLine("feeding");
            Dojodachi dachi = HttpContext.Session.GetObjectFromJson<Dojodachi>("my_dachi");
            Random rand = new Random();

            if(dachi.meals == 0){
                System.Console.WriteLine("out of food....");
                HttpContext.Session.SetString("log", "You are out of meals! Work to earn more meals!");
            } else{
                System.Console.WriteLine("EATING MEAL");
                dachi.meals -= 1;

                if(rand.Next(1,4) == 1){
                    HttpContext.Session.SetString("log","Looks like your dachi didn't like that meal...");
                } else {
                    int newFullness = rand.Next(5, 11);
                    dachi.fullness += newFullness;

                    HttpContext.Session.SetString("log", $"Bulbasaur ate 1 meal and gained {newFullness} fullness!");
                }
                HttpContext.Session.SetObjectAsJson("my_dachi", dachi);
            }
            HttpContext.Session.SetString("action", "background-image: url('../images/bulb-eat.gif')");
            return RedirectToAction("Index");

        }

        // GET: /play/
        [HttpGet]
        [Route("play")]
        public IActionResult Play()
        {
            System.Console.WriteLine("playing");
            Dojodachi dachi = HttpContext.Session.GetObjectFromJson<Dojodachi>("my_dachi");
            Random rand = new Random();

            if(dachi.energy == 0){
                System.Console.WriteLine("out of energy....");
                HttpContext.Session.SetString("log", "Your Dachi is out of energy! He should take a nap.");
            } else {
                dachi.energy -= 5;

                if(rand.Next(1,4) == 1){
                    HttpContext.Session.SetString("log","Seems like Bulbasaur wasn't in the mood to play...");
                } else {
                    int newHappiness = rand.Next(5, 11);
                    dachi.happiness += newHappiness;
                    HttpContext.Session.SetString("log", $"Bulbasaur played for 5 minutes and gained {newHappiness} happiness!");
                }
                HttpContext.Session.SetObjectAsJson("my_dachi", dachi);
            }

            HttpContext.Session.SetString("action", "background-image: url('../images/bulb-play.gif')");
            return RedirectToAction("Index");

        }

        // GET: /work/
        [HttpGet]
        [Route("work")]
        public IActionResult Work()
        {
            System.Console.WriteLine("working");
            Dojodachi dachi = HttpContext.Session.GetObjectFromJson<Dojodachi>("my_dachi");
            Random rand = new Random();

            if(dachi.energy == 0){
                System.Console.WriteLine("out of energy....");
                HttpContext.Session.SetString("log", "Your Dachi is out of energy! He should take a nap.");
            } else {
                dachi.energy -= 5;
                int newMeals = rand.Next(1, 4);
                dachi.meals += newMeals;
                HttpContext.Session.SetString("log", $"Bulbasaur worked and earned {newMeals} meals!");
            }
            HttpContext.Session.SetObjectAsJson("my_dachi", dachi);
            HttpContext.Session.SetString("action", "background-image: url('../images/bulb-work.png')");
            return RedirectToAction("Index");

        }

        // GET: /sleep/
        [HttpGet]
        [Route("sleep")]
        public IActionResult Sleep()
        {
            System.Console.WriteLine("sleeping");
            Dojodachi dachi = HttpContext.Session.GetObjectFromJson<Dojodachi>("my_dachi");

            if(dachi.fullness == 0){
                HttpContext.Session.SetString("log", "Bulbasaur is too hungry to sleep! He will need some food before he can sleep..");
            } else if(dachi.happiness == 0) {
                HttpContext.Session.SetString("log", "Your dachi is too upset to sleep. Play with Bulbasaur to raise his happiness!");
            }else  {
                dachi.fullness -= 5;
                dachi.happiness -= 5;
                dachi.energy += 15;
                HttpContext.Session.SetString("log", "Bulbasaur took a nap and gained 15 energy!");
            }
            HttpContext.Session.SetObjectAsJson("my_dachi", dachi);
            HttpContext.Session.SetString("action", "background-image: url('../images/bulb-sleep.gif')");
            return RedirectToAction("Index");

        }
    }
}

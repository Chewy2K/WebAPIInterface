using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using WebAPIInterface.Models;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WebAPIInterface.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        string baseURL = "http://localhost:8089/";
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //public async Task<IActionResult> Index()
        //{
        //    DataTable dt = new DataTable();
        //    using(var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(baseURL);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        HttpResponseMessage getData = await client.GetAsync("Users/GetUsers");

        //        if (getData.IsSuccessStatusCode)
        //        {
        //            string results = getData.Content.ReadAsStringAsync().Result;
        //            dt=JsonConvert.DeserializeObject<DataTable>(results);
        //        }
        //        else
        //        {
        //            Console.WriteLine("Error 404");
        //        }

        //        ViewData.Model = dt;
        //    }

        //    return View();
        //} //Calling API using Data Table

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Index2() //Calling API yung Entity Model
        {
            IList<UserEntity> users = new List<UserEntity>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("Users/GetUsers");

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    users = JsonConvert.DeserializeObject<List<UserEntity>>(results);
                }
                else
                {
                    Console.WriteLine("Error 404");
                }

                ViewData.Model = users;
            }

            return View();
        }
        public async Task<ActionResult<string>> AddUser(UserEntity user)
        {
            UserEntity obj = new UserEntity()
            {
                Username = user.Username,
                Emailaddress = user.Emailaddress,
                Mobilenumber = user.Mobilenumber,
                Password = user.Password
            };

            if (user.Username != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL + "Users/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.PostAsJsonAsync<UserEntity>("AddUser/adduser", obj);

                    if (getData.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index2","Home");
                    }
                    else
                    {
                        Console.WriteLine("Error 500");
                    }
                }
            } return View();
        }

        public async Task<ActionResult<string>> UpdateUser(UserEntity user)
        {

            if (user.Username != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL + "Users/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.PutAsJsonAsync<UserEntity>("UpdateUser/updateuserdetails", user);

                    if (getData.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index2", "Home");
                    }
                    else
                    {
                        Console.WriteLine("Error 501");
                    }

                }
            }
            return View();
        }

        public async Task<ActionResult<string>> DeleteUser(UserEntity user)
        {
            UserEntity obj = new UserEntity()
            {
                Id = user.Id,
                Username = user.Username,
                Emailaddress = user.Emailaddress,
                Mobilenumber = user.Mobilenumber,
                Password = user.Password
            };

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL + "Users/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.DeleteAsync("DeleteUser/deleteuser?ID=" + user.Id);

                    if (getData.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index2", "Home");
                    }
                    else
                    {
                        Console.WriteLine("Error 502");
                    }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
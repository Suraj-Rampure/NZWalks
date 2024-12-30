using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async  Task<IActionResult> Index()
        {

            List<RegionDto> response = new List<RegionDto>();

            try
            {

                //Get All regions from Web API
                var client = httpClientFactory.CreateClient();

                var httpResponsemessage = await client.GetAsync("https://localhost:7216/api/regions");

                httpResponsemessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponsemessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());

               
            }
            catch (Exception ex)
            {
                //Log the exception
                throw;
            }

            return View(response);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            var client = httpClientFactory.CreateClient();

            var httprequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7216/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };

            var httpresponseMessage =  await client.SendAsync(httprequestMessage);

            httpresponseMessage.EnsureSuccessStatusCode();

            var response = await httpresponseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if(response is not null)
            {
                return RedirectToAction("Index","Regions");
            }

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            var Client = httpClientFactory.CreateClient();

            var response =   await Client.GetFromJsonAsync<RegionDto>($"https://localhost:7216/api/regions/{id.ToString()}");

            if(response is not null)
            {
                return View(response);
            }

            return View();

        }
    }
}

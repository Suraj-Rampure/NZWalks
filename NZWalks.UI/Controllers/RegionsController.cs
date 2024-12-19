using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models.DTO;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }


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
    }
}

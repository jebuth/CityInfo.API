using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly CitiesDataStore _cityDataStore;

        public CitiesController(CitiesDataStore cityDataStore)
        {
            _cityDataStore = cityDataStore ?? throw new ArgumentException(nameof(cityDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return Ok(_cityDataStore.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            var city = _cityDataStore.Cities.FirstOrDefault(c => c.Id.Equals(id));

            if(city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }
    }
}

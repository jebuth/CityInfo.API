using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointOfInterestController : ControllerBase
    {
        private ILogger<PointOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly CitiesDataStore _cityDataStore;

        public PointOfInterestController(ILogger<PointOfInterestController> logger, IMailService mailService, CitiesDataStore cityDataStore)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentException(nameof(mailService));
            _cityDataStore = cityDataStore ?? throw new ArgumentException(nameof(cityDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointOfInterest(int cityId)
        {
            try
            {
                var city = _cityDataStore.Cities.FirstOrDefault(c => c.Id.Equals(cityId));

                if (city == null)
                    return NotFound();

                return Ok(city.PointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.", ex);
                return StatusCode(500, "A problem happenned while handling your request");
            }
        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _cityDataStore.Cities.FirstOrDefault(c => c.Id.Equals(cityId));

            if (city == null)
                return NotFound();

            var poi = city.PointsOfInterest.FirstOrDefault(p => p.Id.Equals(pointOfInterestId));

            if (poi == null)
                return NotFound();

            return Ok(poi);

        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestCreateDto pointOfInterest)
        {
            var city = _cityDataStore.Cities.FirstOrDefault(c => c.Id.Equals(cityId));

            if (city == null)
                return NotFound();

            int maxId = _cityDataStore.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

            var newPointOfInterest = new PointOfInterestDto
            {
                Id = maxId + 1,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description,
            };

            city.PointsOfInterest.Add(newPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterestId = newPointOfInterest.Id
                },
                newPointOfInterest);
        }

        [HttpPut("{pointofinterestid}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestUpdateDto request)
        {
            var city = _cityDataStore.Cities.FirstOrDefault(c => c.Id.Equals(cityId));
            if (city == null) return NotFound();

            var poi = city.PointsOfInterest.FirstOrDefault(p => p.Id.Equals(pointOfInterestId));
            if (poi == null) return NotFound();

            poi.Name = request.Name;
            poi.Description = request.Description;

            return NoContent();
        }

        [HttpPatch("{pointofinterestid}")]
        public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, 
            JsonPatchDocument<PointOfInterestUpdateDto> patchDocument)
        {
            var city = _cityDataStore.Cities.FirstOrDefault(c => c.Id.Equals(cityId));
            if (city == null) return NotFound();

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id.Equals(pointOfInterestId));
            if (pointOfInterestFromStore == null) return NotFound();

            var pointOfInterestToPatch = new PointOfInterestUpdateDto
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!TryValidateModel(pointOfInterestToPatch)) return BadRequest(ModelState);

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent(); 

        }

        [HttpDelete("{pointofinterestid}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _cityDataStore.Cities.FirstOrDefault(c => c.Id.Equals(cityId));
            if (city == null) return NotFound();

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id.Equals(pointOfInterestId));
            if (pointOfInterestFromStore == null) return NotFound();

            city.PointsOfInterest.Remove(pointOfInterestFromStore);

            _mailService.Send("Point of interest deleted", $"Point of interest {pointOfInterestFromStore.Name} deleted.");

            return NoContent();
        }
    }
}

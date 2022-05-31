using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {

        private CityInfoContext context;

        public CityInfoRepository(CityInfoContext _context)
        {
            context = _context ?? throw new ArgumentNullException(nameof(_context));
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await context.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
        {
            if(!includePointsOfInterest)
                return await context.Cities.Where(c => c.Id.Equals(cityId)).FirstOrDefaultAsync();
            
            return await context.Cities.Include(c => c.PointsOfInterest)
                .Where(c => c.Id.Equals(cityId)).FirstOrDefaultAsync();

        }

        public async Task<PointOfInterest> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            return await context.PointOfInterest
                .Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await context.PointOfInterest.Where(p => p.CityId == cityId).ToListAsync();
        }

    }
}

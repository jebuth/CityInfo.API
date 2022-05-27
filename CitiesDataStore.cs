using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "One w big park.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto
                        {
                            Id = 1, 
                            Name = "Central Park",
                            Description = "big park"
                        },
                        new PointOfInterestDto
                        {
                            Id = 2,
                            Name = "Empire State",
                            Description = "big bldg"
                        },
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Dallas",
                    Description = "One w Mavericks.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto
                        {
                            Id = 3,
                            Name = "American Airlines Center",
                            Description = "stadium"
                        },
                        new PointOfInterestDto
                        {
                            Id = 4,
                            Name = "Samurai Museum",
                            Description = "museum"
                        },
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Oakland",
                    Description = "One w warriors.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto
                        {
                            Id = 5,
                            Name = "Oracel Arena",
                            Description = "stadium"
                        },
                        new PointOfInterestDto
                        {
                            Id = 6,
                            Name = "Golden Gate Bridge",
                            Description = "red bridge"
                        },
                    }
                }
            };
        }
    }
}


using SynopticumModel.Entities;

namespace lesson8_WebApi
{
    public static partial class Mocks
    {
        public static List<City> Cities = new List<City>
        {
            new City
            {
                Name = "Hrodna",
                Country = new Country { Name = "Belarus" }
            },
            new City
            {
                Name = "Minsk",
                Country = new Country { Name = "Belarus" }
            },
            new City
            {
                Name = "Tbilisi",
                Country  = new Country { Name = "Georgia" }
            },
            new City
            {
                Name = "Vatican",
                Country  = new Country { Name = "Vatican" }
            }
        };
    }
}

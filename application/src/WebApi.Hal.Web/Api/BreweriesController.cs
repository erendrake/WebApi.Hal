using Microsoft.AspNet.Mvc;
using System.Linq;
using WebApi.Hal.Web.Api.Resources;
using WebApi.Hal.Web.Data;

namespace WebApi.Hal.Web.Api
{
    [Route(Constants.EndPoints.Breweries)]
    public class BreweriesController : Controller
    {
        readonly BeerDbContext beerDbContext;

        public BreweriesController(BeerDbContext beerDbContext)
        {
            this.beerDbContext = beerDbContext;
        }

        [HttpGet]
        public BreweryListRepresentation Get()
        {
            var breweries = beerDbContext.Styles
                .ToList()
                .Select(s => new BreweryRepresentation
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToList();

            return new BreweryListRepresentation(breweries);
        }

        /*
        [HttpGet]
        public BreweryRepresentation Get(int id)
        {
            var brewery = beerDbContext.Breweries.FirstOrDefault(b => b.Id == id);
            return new BreweryRepresentation
            {
                Id = brewery.Id,
                Name = brewery.Name
            };
        }
        */
    }
}
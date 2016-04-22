using Microsoft.AspNet.Mvc;
using System.Linq;
using WebApi.Hal.Web.Api.Resources;
using WebApi.Hal.Web.Data;
using WebApi.Hal.Web.Data.Queries;

namespace WebApi.Hal.Web.Api
{
    [Route(Constants.EndPoints.BeersFromBrewery)]
    public class BeersFromBreweryController : Controller
    {
        readonly IRepository repository;

        public BeersFromBreweryController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public BeerListRepresentation Get(int id, int page = 1)
        {
            var beers = repository.Find(new GetBeersQuery(b => b.Brewery.Id == id), page, BeersController.PageSize);
            return new BeerListRepresentation(beers.ToList(), beers.TotalResults, beers.TotalPages, page, LinkTemplates.Breweries.AssociatedBeers, new { id });
        }
    }
}
using Microsoft.AspNet.Mvc;
using System.Linq;
using WebApi.Hal.Web.Api.Resources;
using WebApi.Hal.Web.Data;
using WebApi.Hal.Web.Data.Queries;
using WebApi.Hal.Web.Models;

namespace WebApi.Hal.Web.Api
{
    [Route(Constants.EndPoints.Beers)]
    public class BeersController : Controller
    {
        public const int PageSize = 5;

        private readonly IRepository repository;

        public BeersController(IRepository repository)
        {
            this.repository = repository;
        }

        // GET beers
        [HttpGet]
        public BeerListRepresentation Get(int page = 1)
        {
            PagedResult<BeerRepresentation> beers = repository.Find(new GetBeersQuery(), page, PageSize);

            var resourceList = new BeerListRepresentation(beers.ToList(), beers.TotalResults, beers.TotalPages, page, LinkTemplates.Beers.GetBeers);

            return resourceList;
        }

        /*
        [HttpGet]
        public BeerListRepresentation Search(string searchTerm, int page = 1)
        {
            var beers = repository.Find(new GetBeersQuery(b => b.Name.Contains(searchTerm)), page, PageSize);

            // snap page back to actual page found
            if (page > beers.TotalPages) page = beers.TotalPages;

            //var link = LinkTemplates.Beers.SearchBeers.CreateLink(new { searchTerm, page });
            var beersResource = new BeerListRepresentation(beers.ToList(), beers.TotalResults, beers.TotalPages, page,
                                                           LinkTemplates.Beers.SearchBeers,
                                                           new { searchTerm })
            {
                Page = page,
                TotalResults = beers.TotalResults
            };

            return beersResource;
        }
        */

        [HttpPost]
        // POST beers
        public ActionResult Post(BeerRepresentation value)
        {
            var newBeer = new Beer(value.Name);
            repository.Add(newBeer);
            return new CreatedResult(LinkTemplates.Beers.Beer.CreateUri(new { id = newBeer.Id }), newBeer.Id);
        }        
    }
}
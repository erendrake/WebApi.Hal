using Microsoft.AspNet.Mvc;
using System.Linq;
using System.Net;
using WebApi.Hal.Web.Api.Resources;
using WebApi.Hal.Web.Data;

namespace WebApi.Hal.Web.Api
{
    [Route(Constants.EndPoints.Styles)]
    public class StylesController : Controller
    {
        readonly BeerDbContext beerDbContext;

        public StylesController(BeerDbContext beerDbContext)
        {
            this.beerDbContext = beerDbContext;
        }

        [HttpGet]
        public BeerStyleListRepresentation Get()
        {
            var beerStyles = beerDbContext.Styles
                .ToList()
                .Select(s => new BeerStyleRepresentation
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToList();

            return new BeerStyleListRepresentation(beerStyles);
        }

        /*
        [HttpGet]
        public ActionResult Get(int id)
        {
            var beerStyle = beerDbContext.Styles.SingleOrDefault(s => s.Id == id);
            if (beerStyle == null)
                return new HttpNotFoundResult();

            var beerStyleResource = new BeerStyleRepresentation
            {
                Id = beerStyle.Id,
                Name = beerStyle.Name
            };

            return new HttpOkObjectResult(beerStyleResource);
        }
        */
    }
}
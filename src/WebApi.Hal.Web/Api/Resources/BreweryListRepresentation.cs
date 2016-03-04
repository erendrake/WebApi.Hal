using System.Collections.Generic;
using iUS.WebApi.Hal;

namespace WebApi.Hal.Web.Api.Resources
{
    public class BreweryListRepresentation : SimpleListRepresentation<BreweryRepresentation>
    {
        public BreweryListRepresentation(IList<BreweryRepresentation> breweries)
            : base(breweries)
        {
        }

        protected override void CreateHypermedia()
        {
            Href = LinkTemplates.Breweries.GetBreweries.Href;

            Links.Add(new Link { Href = Href, Rel = "self" });
        }
    }
}
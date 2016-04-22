using System.Collections.Generic;
using iUS.WebApi.Hal.Interfaces;
using iUS.WebApi.Hal.Tests.Representations;

namespace iUS.WebApi.Hal.Tests.HypermediaAppenders
{
    public class ProductRepresentationHypermediaAppender : IHypermediaAppender<ProductRepresentation>
    {
        public void Append(ProductRepresentation resource, IEnumerable<Link> configured)
        {
            foreach (Link link in configured)
            {
                switch (link.Rel)
                {
                    case Link.RelForSelf:
                        resource.Links.Add(link.CreateLink(new { id = resource.Id }));
                        break;
                    case "example-namespace:category":
                        resource.Links.Add(link.CreateLink(new {id = "Action Figures"}));
                        break;
                    case "example-namespace:related-product":
                        for (var i = 0; i < 3; i++)
                            resource.Links.Add(link.CreateLink(new { id = $"related-product-{i:00}"}));
                        break;
                    case "example-namespace:product-on-sale":
                        for (var i = 0; i < 3; i++)
                            resource.Links.Add(link.CreateLink(new { id = $"product-on-sale-{i:00}"}));
                        break;
                    default:
                        resource.Links.Add(link); // append untouched ...
                        break;
                }
            }
        }
    }
}
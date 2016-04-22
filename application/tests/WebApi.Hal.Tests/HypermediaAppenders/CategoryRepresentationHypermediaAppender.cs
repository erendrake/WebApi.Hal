using System.Collections.Generic;
using iUS.WebApi.Hal.Interfaces;
using iUS.WebApi.Hal.Tests.Representations;

namespace iUS.WebApi.Hal.Tests.HypermediaAppenders
{
    public class CategoryRepresentationHypermediaAppender : IHypermediaAppender<CategoryRepresentation>
    {
        public void Append(CategoryRepresentation resource, IEnumerable<Link> configured)
        {
            foreach (Link link in configured)
            {
                switch (link.Rel)
                {
                    case Link.RelForSelf:
                        resource.Links.Add(link.CreateLink(new { id = resource.Id }));
                        break;
                    default:
                        resource.Links.Add(link); // append untouched ...
                        break;
                }
            }
        }
    }
}
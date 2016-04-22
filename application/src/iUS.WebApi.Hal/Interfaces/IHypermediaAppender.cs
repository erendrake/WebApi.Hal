using System.Collections.Generic;

namespace iUS.WebApi.Hal.Interfaces
{
    public interface IHypermediaAppender<in T> where T:class, IResource
    {
        void Append(T resource, IEnumerable<Link> configured);
    }
}
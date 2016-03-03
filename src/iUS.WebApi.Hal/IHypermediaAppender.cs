using System.Collections.Generic;
using iUS.WebApi.Hal.Interfaces;

namespace iUS.WebApi.Hal
{
    public interface IHypermediaAppender<T> where T:class, IResource
    {
        void Append(T resource, IEnumerable<Link> configured);
    }
}
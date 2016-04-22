using System.Collections.Generic;
using iUS.WebApi.Hal.Interfaces;

namespace iUS.WebApi.Hal
{
    public abstract class SimpleListRepresentation<TResource> : Representation where TResource : IResource
    {
        protected SimpleListRepresentation()
        {
            Items = new List<TResource>();
        }

        protected SimpleListRepresentation(IList<TResource> list)
        {
            Items = list;
        }

        public IList<TResource> Items { get; set; }
    }
}

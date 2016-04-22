using System.Collections.Generic;
using iUS.WebApi.Hal.Interfaces;

namespace iUS.WebApi.Hal
{
    internal class EmbeddedResource
    {
        public EmbeddedResource()
        {
            Resources = new List<IResource>();
        }

        public bool IsSourceAnArray { get; set; }
        public IList<IResource> Resources { get; private set; }
    }
}
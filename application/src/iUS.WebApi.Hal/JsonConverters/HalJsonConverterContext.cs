using System;
using iUS.WebApi.Hal.Interfaces;

namespace iUS.WebApi.Hal.JsonConverters
{
    public class HalJsonConverterContext
    {
        public HalJsonConverterContext()
        {
            IsRoot = true;
        }

        public HalJsonConverterContext(IHypermediaResolver hypermediaResolver) : this()
        {
            if (hypermediaResolver == null) 
                throw new ArgumentNullException(nameof(hypermediaResolver));

            this.HypermediaResolver = hypermediaResolver;
        }

        public IHypermediaResolver HypermediaResolver { get; }

        public bool IsRoot { get; set; }
    }
}
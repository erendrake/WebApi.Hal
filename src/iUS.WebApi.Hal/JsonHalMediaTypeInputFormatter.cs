using System;
using iUS.WebApi.Hal.Interfaces;
using iUS.WebApi.Hal.JsonConverters;
using Microsoft.AspNet.Mvc.Formatters;
using Newtonsoft.Json;

namespace iUS.WebApi.Hal
{
    public class JsonHalMediaTypeInputFormatter : JsonInputFormatter
    {
        private readonly LinksConverter linksConverter = new LinksConverter();

        private readonly ResourceConverter resourceConverter = new ResourceConverter();

        private readonly EmbeddedResourceConverter embeddedResourceConverter = new EmbeddedResourceConverter();

        #region Constructors

        public JsonHalMediaTypeInputFormatter(IHypermediaResolver hypermediaResolver)
        {
            if (hypermediaResolver == null)
            {
                throw new ArgumentNullException(nameof(hypermediaResolver));
            }

            resourceConverter = new ResourceConverter(hypermediaResolver);
            Initialize();
        }

        public JsonHalMediaTypeInputFormatter()
        {
            Initialize();
        }

        #endregion

        #region Private methods

        private void Initialize()
        {
            SerializerSettings.Converters.Add(linksConverter);
            SerializerSettings.Converters.Add(resourceConverter);
            SerializerSettings.Converters.Add(embeddedResourceConverter);
            SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }

        #endregion
    }
}

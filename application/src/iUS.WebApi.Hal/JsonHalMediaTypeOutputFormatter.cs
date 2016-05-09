using iUS.WebApi.Hal.Interfaces;
using iUS.WebApi.Hal.JsonConverters;
using Microsoft.AspNet.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace iUS.WebApi.Hal
{
    public class JsonHalMediaTypeOutputFormatter : JsonOutputFormatter
    {
        private const string MediaTypeHeaderValueName = "application/hal+json";

        private readonly LinksConverter linksConverter = new LinksConverter();

        private readonly ResourceConverter resourceConverter = new ResourceConverter();

        private readonly EmbeddedResourceConverter embeddedResourceConverter = new EmbeddedResourceConverter();

        #region Constructors

        public JsonHalMediaTypeOutputFormatter(IHypermediaResolver hypermediaResolver)
        {
            if (hypermediaResolver == null)
            {
                throw new ArgumentNullException(nameof(hypermediaResolver));
            }

            resourceConverter = new ResourceConverter(hypermediaResolver);
            Initialize();
        }

        public JsonHalMediaTypeOutputFormatter()
        {
            Initialize();
        }

        #endregion Constructors

        #region Private methods

        private void Initialize()
        {
            SupportedMediaTypes.Clear();
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(MediaTypeHeaderValueName));
            SerializerSettings.Converters.Add(linksConverter);
            SerializerSettings.Converters.Add(resourceConverter);
            SerializerSettings.Converters.Add(embeddedResourceConverter);
            SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        #endregion Private methods
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace iUS.WebApi.Hal
{
    public class XmlHalMediaTypeOutputFormatter : OutputFormatter
    {
        private const string MediaTypeHeaderValueName = "application/hal+xml";

        #region Constructor

        public XmlHalMediaTypeOutputFormatter()
        {
            Initialize();
        }

        #endregion

        #region Public methods

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            HttpResponse response = context.HttpContext.Response;
            MediaTypeHeaderValue contentType = context.ContentType;
            Encoding encoding = contentType?.Encoding ?? Encoding.UTF8;
            using (TextWriter textWriter = context.WriterFactory(response.Body, encoding))
            {
                WriteObject(textWriter, context.Object);
            }

            return Task.FromResult(true);
        }

        public void WriteObject(TextWriter writer, object value)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var representation = value as Representation;
            if (representation == null)
            {
                return;
            }

            var settings = new XmlWriterSettings
            {
                Indent = true
            };
            using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
            {
                WriteHalResource(representation, xmlWriter);
                xmlWriter.Flush();
            }
        }

        #endregion

        #region Private methods

        private void Initialize()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(MediaTypeHeaderValueName));
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// ReadHalResource will
        /// </summary>
        /// <param name="type">Type of resource - Must be of type ApiResource</param>
        /// <param name="xml">xelement for the type</param>
        /// <returns>returns deserialized object</returns>
        private static object ReadHalResource(Type type, XElement xml)
        {
            if (xml == null)
            {
                return null;
            }

            // First, determine if Resource of type Generic List and construct Instance with respective Parameters
            var representation = Activator.CreateInstance(type) as Representation;

            // Second, set the well-known HAL properties ONLY if type of Resource
            CreateSelfHypermedia(type, xml, representation);

            // Third, read/set the rest of the properties
            SetProperties(type, xml, representation);

            // Fourth, read each link element
            IEnumerable<XElement> links = xml.Elements("link");
            List<Link> linksList = links.Select(link => new Link
            {
                Rel = link.Attribute("rel").Value,
                Href = link.Attribute("href").Value
            }).ToList();

            type.GetProperty("Links").SetValue(representation, linksList, null);

            return representation;
        }

        private static void SetProperties(Type type, XElement xml, Representation representation)
        {
            foreach (PropertyInfo property in type.GetPublicInstanceProperties())
            {
                if (property.IsValidBasicType())
                {
                    type.SetPropertyValue(property.Name, xml.Element(property.Name), representation);
                }
                else if (typeof(Representation).IsAssignableFrom(property.PropertyType) &&
                         property.GetIndexParameters().Length == 0)
                {
                    XElement resourceXml =
                        xml.Elements("resource").SingleOrDefault(x => x.Attribute("name").Value == property.Name);
                    object halResource = ReadHalResource(property.PropertyType, resourceXml);
                    property.SetValue(representation, halResource, null);
                }
            }
        }

        private static void CreateSelfHypermedia(Type type, XElement xml, Representation representation)
        {
            type.GetProperty("Rel").SetValue(representation, xml.Attribute("rel").Value, null);
            type.SetPropertyValue("Href", xml.Attribute("href"), representation);
            type.SetPropertyValue("LinkName", xml.Attribute("name"), representation);
        }

        private static void WriteHalResource(Representation representation, XmlWriter writer, string propertyName = null)
        {
            if (representation == null)
            {
                return;
            }

            representation.RepopulateHyperMedia();

            // First write the well-known HAL properties
            writer.WriteStartElement("resource");
            writer.WriteAttributeString("rel", representation.Rel);
            writer.WriteAttributeString("href", representation.Href);
            if (representation.LinkName != null || propertyName != null)
            {
                writer.WriteAttributeString("name", representation.LinkName = representation.LinkName ?? propertyName);
            }

            // Second write out the links of the resource
            var links = new HashSet<Link>(representation.Links.Where(link => link.Rel != "self"), Link.EqualityComparer);
            foreach (Link link in links)
            {
                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", link.Rel);
                writer.WriteAttributeString("href", link.Href);
                writer.WriteEndElement();
            }

            // Third, write the rest of the properties
            WriteResourceProperties(representation, writer);

            writer.WriteEndElement();
        }

        private static void WriteResourceProperties(Representation representation, XmlWriter writer)
        {
            // Only simple type and nested ApiResource type will be handled : for any other type, exception will be thrown
            // including List<ApiResource> as representation of List would require properties rel, href and linkname
            // To overcome the issue, use "RepresentationList<T>"
            foreach (PropertyInfo property in representation.GetType().GetPublicInstanceProperties())
            {
                if (property.IsValidBasicType())
                {
                    string propertyString = GetPropertyString(property, representation);
                    if (propertyString != null)
                    {
                        writer.WriteElementString(property.Name, propertyString);
                    }
                }
                else if (typeof (Representation).IsAssignableFrom(property.PropertyType) &&
                         property.GetIndexParameters().Length == 0)
                {
                    object halResource = property.GetValue(representation, null);
                    WriteHalResource((Representation) halResource, writer, property.Name);
                }
                else if (typeof (IEnumerable<Representation>).IsAssignableFrom(property.PropertyType))
                {
                    var halResourceList = property.GetValue(representation, null) as IEnumerable<Representation>;
                    if (halResourceList != null)
                        foreach (Representation item in halResourceList)
                            WriteHalResource(item, writer);
                }
            }
        }

        private static string GetPropertyString(PropertyInfo property, object instance)
        {
            object propertyValue = property.GetValue(instance, null);
            return propertyValue?.ToString();
        }

        #endregion
    }
}
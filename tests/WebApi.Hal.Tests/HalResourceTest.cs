using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using iUS.WebApi.Hal.Tests.Representations;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Abstractions;
using Microsoft.AspNet.Mvc.Formatters;
using Microsoft.AspNet.Mvc.Infrastructure;
using Microsoft.AspNet.Routing;
using Microsoft.Net.Http.Headers;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace iUS.WebApi.Hal.Tests
{
    public class HalResourceTest
    {
        readonly OrganisationRepresentation resource;

        public HalResourceTest()
        {
            resource = new OrganisationRepresentation(1, "Org Name");
        }
        
        [Fact]
        [UseReporter(typeof(DiffReporter))]
        public async Task organisation_get_json_test()
        {
            // arrange
            var mediaFormatter = new JsonHalMediaTypeOutputFormatter()
            {
                SerializerSettings = { Formatting = Formatting.Indented }
            };

            var context = GetOutputFormatterContext(resource, typeof(OrganisationRepresentation));

            // act
            await mediaFormatter.WriteResponseBodyAsync(context);

            var body = context.HttpContext.Response.Body;

            Assert.NotNull(body);
            body.Position = 0;

            var content = new StreamReader(body, Encoding.UTF8).ReadToEnd();

            // assert
            Approvals.Verify(content);
        }
        
        [Fact]
        [UseReporter(typeof(DiffReporter))]
        public async Task organisation_get_json_with_app_path_test()
        {
            // arrange
            var mediaFormatter = new JsonHalMediaTypeOutputFormatter()
            {
                SerializerSettings = { Formatting = Formatting.Indented }
            };

            var resourceWithAppPath = new OrganisationWithAppPathRepresentation(1, "Org Name");

            var context = GetOutputFormatterContext(resourceWithAppPath, typeof(OrganisationWithAppPathRepresentation));

            // act
            await mediaFormatter.WriteResponseBodyAsync(context);

            var body = context.HttpContext.Response.Body;

            Assert.NotNull(body);
            body.Position = 0;

            var content = new StreamReader(body, Encoding.UTF8).ReadToEnd();

            // assert
            Approvals.Verify(content);
        }
        
        [Fact]
        [UseReporter(typeof(DiffReporter))]
        public async Task organisation_get_json_with_no_href_test()
        {
            // arrange
            var mediaFormatter = new JsonHalMediaTypeOutputFormatter()
            {
                SerializerSettings = { Formatting = Formatting.Indented }
            };

            var resourceWithAppPath = new OrganisationWithNoHrefRepresentation(1, "Org Name");

            var context = GetOutputFormatterContext(resourceWithAppPath, typeof(OrganisationWithNoHrefRepresentation));

            // act
            await mediaFormatter.WriteResponseBodyAsync(context);

            var body = context.HttpContext.Response.Body;

            Assert.NotNull(body);
            body.Position = 0;

            var content = new StreamReader(body, Encoding.UTF8).ReadToEnd();

            // assert
            Approvals.Verify(content);
        }
        
        [Fact]
        [UseReporter(typeof(DiffReporter))]
        public async Task organisation_get_json_with_link_title_test()
        {
            // arrange
            var mediaFormatter = new JsonHalMediaTypeOutputFormatter()
            {
                SerializerSettings = { Formatting = Formatting.Indented }
            };

            var resourceWithAppPath = new OrganisationWithLinkTitleRepresentation(1, "Org Name");

            var context = GetOutputFormatterContext(resourceWithAppPath, typeof(OrganisationWithNoHrefRepresentation));

            // act
            await mediaFormatter.WriteResponseBodyAsync(context);

            var body = context.HttpContext.Response.Body;

            Assert.NotNull(body);
            body.Position = 0;

            var content = new StreamReader(body, Encoding.UTF8).ReadToEnd();

            // assert
            Approvals.Verify(content);
        }
        
        [Fact]
        [UseReporter(typeof(DiffReporter))]
        public async Task organisation_get_xml_test()
        {
            // arrange
            var mediaFormatter = new XmlHalMediaTypeOutputFormatter();

            var context = GetOutputFormatterContext(resource, typeof(OrganisationRepresentation));

            // act
            await mediaFormatter.WriteResponseBodyAsync(context);

            var body = context.HttpContext.Response.Body;

            Assert.NotNull(body);
            body.Position = 0;

            var content = new StreamReader(body, Encoding.UTF8).ReadToEnd();

            // assert
            Approvals.Verify(content);
        }

        public static OutputFormatterWriteContext GetOutputFormatterContext(
            object outputValue,
            Type outputType,
            string contentType = "application/hal+json",
            MemoryStream responseStream = null)
        {
            var mediaTypeHeaderValue = MediaTypeHeaderValue.Parse(contentType);

            var actionContext = GetActionContext(mediaTypeHeaderValue, responseStream);
            return new OutputFormatterWriteContext(
                actionContext.HttpContext,
                new TestHttpResponseStreamWriterFactory().CreateWriter,
                outputType,
                outputValue)
            {
                ContentType = mediaTypeHeaderValue,
            };
        }

        public static ActionContext GetActionContext(
            MediaTypeHeaderValue contentType,
            MemoryStream responseStream = null)
        {
            var request = new Mock<HttpRequest>();
            var headers = new HeaderDictionary();
            request.Setup(r => r.ContentType).Returns(contentType.ToString());
            request.SetupGet(r => r.Headers).Returns(headers);
            headers[HeaderNames.AcceptCharset] = contentType.Charset;
            var response = new Mock<HttpResponse>();
            response.SetupGet(f => f.Body).Returns(responseStream ?? new MemoryStream());
            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(c => c.Request).Returns(request.Object);
            httpContext.SetupGet(c => c.Response).Returns(response.Object);
            return new ActionContext(httpContext.Object, new RouteData(), new ActionDescriptor());
        }

        public class TestHttpResponseStreamWriterFactory : IHttpResponseStreamWriterFactory
        {
            public TextWriter CreateWriter(Stream stream, Encoding encoding)
            {
                return new HttpResponseStreamWriter(stream, encoding);
            }
        }
    }
}
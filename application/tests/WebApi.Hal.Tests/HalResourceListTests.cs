using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using iUS.WebApi.Hal.Tests.Representations;
using Newtonsoft.Json;
using Xunit;

namespace iUS.WebApi.Hal.Tests
{
    public class HalResourceListTests
    {
        private readonly OrganisationListRepresentation representation;

        private readonly OrganisationListRepresentation oneitemrepresentation;

        public HalResourceListTests()
        {
            representation = new OrganisationListRepresentation(
                new List<OrganisationRepresentation>
                       {
                           new OrganisationRepresentation(1, "Org1"),
                           new OrganisationRepresentation(2, "Org2")
                       });

            oneitemrepresentation = new OrganisationListRepresentation(
                new List<OrganisationRepresentation>
                       {
                           new OrganisationRepresentation(1, "Org1")
                       });
        }

        [Fact]
        [UseReporter(typeof(DiffReporter))]
        public async Task organisation_list_get_xml_test()
        {
            // arrange
            var mediaFormatter = new XmlHalMediaTypeOutputFormatter();
            
            var context = HalResourceTest.GetOutputFormatterContext(representation, typeof(OrganisationListRepresentation));

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
        public async Task organisation_list_get_json_test()
        {
            // arrange
            var mediaFormatter = new JsonHalMediaTypeOutputFormatter()
            {
                SerializerSettings = { Formatting = Formatting.Indented}
            };

            var context = HalResourceTest.GetOutputFormatterContext(representation, typeof(OrganisationListRepresentation));

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
        public async Task one_item_organisation_list_get_json_test()
        {
            // arrange
            var mediaFormatter = new JsonHalMediaTypeOutputFormatter
            {
                SerializerSettings = {Formatting = Formatting.Indented}
            };

            var context = HalResourceTest.GetOutputFormatterContext(oneitemrepresentation, typeof(OrganisationListRepresentation));

            // act
            
            await mediaFormatter.WriteResponseBodyAsync(context);

            var body = context.HttpContext.Response.Body;

            Assert.NotNull(body);
            body.Position = 0;

            var content = new StreamReader(body, Encoding.UTF8).ReadToEnd();

            // assert
            Approvals.Verify(content);
            
        }
    }
}
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
    public class HalResourceWithPeopleTest
    {
        readonly OrganisationWithPeopleRepresentation resource;

        public HalResourceWithPeopleTest()
        {
            resource = new OrganisationWithPeopleRepresentation(1, "Org Name");
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

            var context = HalResourceTest.GetOutputFormatterContext(resource, typeof(OrganisationWithPeopleRepresentation));

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

            var context = HalResourceTest.GetOutputFormatterContext(resource, typeof(OrganisationWithPeopleRepresentation));

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
using ApprovalTests;
using ApprovalTests.Reporters;
using iUS.WebApi.Hal;
using System.Collections.Generic;
using System.IO;
using WebApi.Hal.Tests.Representations;
using Xunit;

namespace WebApi.Hal.Tests
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
        public void organisation_list_get_xml_test()
        {
            // arrange
            var mediaFormatter = new XmlHalMediaTypeOutputFormatter();

            // act
            using (var stream = new StringWriter())
            {
                mediaFormatter.WriteObject(stream, representation);

                string serialisedResult = stream.ToString();

                // assert
                Approvals.Verify(serialisedResult);
            }
        }

        [Fact]
        [UseReporter(typeof(DiffReporter))]
        public void organisation_list_get_json_test()
        {
            // arrange
            var mediaFormatter = new JsonHalMediaTypeOutputFormatter();

            // act
            using (var stream = new StringWriter())
            {
                mediaFormatter.WriteObject(stream, representation);

                string serialisedResult = stream.ToString();

                // assert
                Approvals.Verify(serialisedResult);
            }
        }

        [Fact]
        [UseReporter(typeof(DiffReporter))]
        public void one_item_organisation_list_get_json_test()
        {
            // arrange
            var mediaFormatter = new JsonHalMediaTypeOutputFormatter();

            // act
            using (var stream = new StringWriter())
            {
                mediaFormatter.WriteObject(stream, oneitemrepresentation);

                string serialisedResult = stream.ToString();

                // assert
                Approvals.Verify(serialisedResult);
            }
        }
    }
}
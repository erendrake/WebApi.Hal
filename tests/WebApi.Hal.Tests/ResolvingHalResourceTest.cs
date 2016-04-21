using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using iUS.WebApi.Hal;
using Newtonsoft.Json;
using WebApi.Hal.Tests.HypermediaAppenders;
using WebApi.Hal.Tests.Representations;
using Xunit;

namespace WebApi.Hal.Tests
{
    public class ResolvingHalResourceTest
    {
        readonly ProductRepresentation representation;

        public ResolvingHalResourceTest()
        {
            //
            // Create representation

            representation = new ProductRepresentation
            {
                Id = 9,
                Title = "Morpheus in a chair statuette",
                Price = 20.14,
                Category = new CategoryRepresentation
                {
                    Id = 99,
                    Title = "Action Figures"
                }
            };
        }
        
        [Fact]
        [UseReporter(typeof(DiffReporter))]
        public async Task ProperlySerializesRepresentationToJson()
        {
			// arrange
			var example = new CuriesLink("example-namespace", "http://api.example.com/docs/{rel}");
			var productLink = example.CreateLink("product", "http://api.example.com/products/{id}");
			var relatedProductLink = example.CreateLink("related-product", productLink.Href);
			var saleProductLink = example.CreateLink("product-on-sale", "http://api.example.com/products/sale/{id}");
			var categoryLink = example.CreateLink("category", "http://api.example.com/categories/{id}");

			var builder = Hypermedia.CreateBuilder();

			builder.RegisterAppender(new ProductRepresentationHypermediaAppender());
			builder.RegisterAppender(new CategoryRepresentationHypermediaAppender());

			builder.RegisterSelf<ProductRepresentation>(productLink);
			builder.RegisterSelf<CategoryRepresentation>(categoryLink);
			builder.RegisterLinks<ProductRepresentation>(relatedProductLink, saleProductLink);

			var config = builder.Build();
			var mediaFormatter = new JsonHalMediaTypeOutputFormatter(config)
            {
                SerializerSettings = { Formatting = Formatting.Indented }
            };

            var context = HalResourceTest.GetOutputFormatterContext(representation, typeof(ProductRepresentation));

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
		public async Task ProperlySerializesRepresentationWithoutLinksToJson()
		{
			// arrange
			var builder = Hypermedia.CreateBuilder();
			var config = builder.Build();
            var mediaFormatter = new JsonHalMediaTypeOutputFormatter(config)
            {
                SerializerSettings = { Formatting = Formatting.Indented }
            };

            var context = HalResourceTest.GetOutputFormatterContext(representation, typeof(ProductRepresentation));

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
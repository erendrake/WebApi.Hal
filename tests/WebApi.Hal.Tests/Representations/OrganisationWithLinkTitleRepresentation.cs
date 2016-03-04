using iUS.WebApi.Hal;

namespace WebApi.Hal.Tests.Representations
{
    /// <summary>
    /// link title
    /// </summary>
    public class OrganisationWithLinkTitleRepresentation : Representation
    {
        private static readonly Link withAppPath = new Link("organisation", "~/api/organisations/{0}");

        public OrganisationWithLinkTitleRepresentation(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string Rel => withAppPath.Rel;

        public override string Href => null;

        public int Id { get; set; }
        public string Name { get; set; }

        protected override void CreateHypermedia()
        {
            Links.Add(new Link("someRel", "someHref", "someTitle"));
        }
    }
}
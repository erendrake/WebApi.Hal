namespace iUS.WebApi.Hal.Tests.Representations
{
    public class OrganisationWithAppPathRepresentation : Representation
    {
        private static readonly Link withAppPath = new Link("organisation", "~/api/organisations/{0}");

        public OrganisationWithAppPathRepresentation(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string Rel => withAppPath.Rel;

        public override string Href => string.Format(withAppPath.Href, Id);

        public int Id { get; set; }
        public string Name { get; set; }

        protected override void CreateHypermedia()
        {
        }
    }
}
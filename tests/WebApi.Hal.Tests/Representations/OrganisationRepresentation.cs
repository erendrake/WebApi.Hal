namespace iUS.WebApi.Hal.Tests.Representations
{
    public class OrganisationRepresentation : Representation
    {
        private static readonly Link noAppPath = new Link("organisation", "/api/organisations/{0}");

        public OrganisationRepresentation(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string Rel
        {
            get { return noAppPath.Rel; }
            set { }
        }

        public override string Href
        {
            get { return string.Format(noAppPath.Href, Id); }
            set { }
        }

        public int Id { get; set; }
        public string Name { get; set; }

        protected override void CreateHypermedia()
        {
        }
    }
}
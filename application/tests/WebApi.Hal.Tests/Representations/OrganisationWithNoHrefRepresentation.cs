namespace iUS.WebApi.Hal.Tests.Representations
{
    /// <summary>
    /// no self link is desired, as is the case when a client generates a represent to send to the server
    /// </summary>
    public class OrganisationWithNoHrefRepresentation : Representation
    {
        private static readonly Link withAppPath = new Link("organisation", "~/api/organisations/{0}");

        public OrganisationWithNoHrefRepresentation(int id, string name)
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
        }
    }
}
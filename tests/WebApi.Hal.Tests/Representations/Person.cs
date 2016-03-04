using iUS.WebApi.Hal;

namespace WebApi.Hal.Tests.Representations
{
    public class Person : Representation
    {
        public Person(int id, string name, int orgId)
        {
            Id = id;
            Name = name;
            OrganisationId = orgId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int OrganisationId { get; set; }

        public override string Rel => "person";

        public override string Href => $"/api/organisations/{OrganisationId}/people/{Id}";

        protected override void CreateHypermedia()
        {
        }
    }
}
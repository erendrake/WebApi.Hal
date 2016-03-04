using System.Collections.Generic;
using iUS.WebApi.Hal;

namespace WebApi.Hal.Tests.Representations
{
    public class OrganisationWithPeopleDetailRepresentation : Representation
    {
        public OrganisationWithPeopleDetailRepresentation(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public IList<Person> People { get; set; }
        public Boss Boss { get; set; }

        public override string Rel => "organisation";

        public override string Href => $"/api/organisations/{Id}";

        protected override void CreateHypermedia()
        {
            var l = new Link
            {
                Rel = "people",
                Href = $"/api/organisations/{Id}/people"
            };
            Links.Add(l);
            // intentionally add a duplicate to make sure it gets screened out
            Links.Add(l);
        }
    }
}

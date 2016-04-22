namespace iUS.WebApi.Hal.Tests.Representations
{
    public class Boss : Person // it's debatable whether some bosses are people
    {
        public Boss(int id, string name, int orgId, bool hasPointHair) : base(id, name, orgId)
        {
            HasPointyHair = hasPointHair;
        }

        public bool HasPointyHair { get; set; }

        public override string Rel => "boss";

        public override string Href => $"/api/organisations/{OrganisationId}/boss";

        protected override void CreateHypermedia()
        {
        }
    }
}
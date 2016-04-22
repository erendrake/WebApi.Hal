namespace WebApi.Hal.Web.Data
{
    public interface ICommand
    {
        void Execute(BeerDbContext dbContext);
    }
}
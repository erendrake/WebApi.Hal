namespace WebApi.Hal.Web.Data
{
    public interface IPagedQuery<T>
    {
        PagedResult<T> Execute(BeerDbContext dbContext, int skip, int take);
    }
}
namespace OrderApp.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IOrderRepository Orders { get; }
        IArticleRepository Articles { get; }
        IOrderArticleRepository OrderArticles { get; }

        Task Save();
    }
}

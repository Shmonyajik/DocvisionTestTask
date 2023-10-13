namespace LettersRegistration.WebAPI.DAL
{
    public interface IBaseRepository<T> : IDisposable
    {
        Task Create(T model);
        Task CreateMultiple(IEnumerable<T> model);

        IQueryable<T> GetBy();

        Task<IEnumerable<T>> GetAll();

        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetBySender(string sender);


    }
}

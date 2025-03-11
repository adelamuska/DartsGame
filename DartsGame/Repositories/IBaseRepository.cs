

namespace DartsGame.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(Guid Id);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task Delete(Guid Id);
    }
}

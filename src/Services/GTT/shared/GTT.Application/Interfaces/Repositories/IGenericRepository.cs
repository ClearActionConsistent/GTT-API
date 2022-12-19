using GTT.Application.ViewModels;
using GTT.Domain.Entities;

namespace GTT.Application.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<ChallengeVM>> GetAllPagingAsync(int pageindex, int pagesize);
        Task<ChallengeVM> AddAsync(ChallengeVM challenge);
        Task<int> DeleteAsync(int id);
        Task<int> UpdateAsync(T entity);
    }
}

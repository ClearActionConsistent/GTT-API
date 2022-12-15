using GTT.Application.Responses;
using GTT.Application.ViewModels;
using GTT.Domain.Entities;

namespace GTT.Application.Repositories
{
    public interface IClassRepository : IGenericRepository<Challenge>
    {
        Task<ListResponse<ClassesVM>> GetListClassByActivity(int ActId);
    }
}
 
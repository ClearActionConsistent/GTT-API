using GTT.Application.Requests;
using GTT.Application.Response;
using GTT.Domain.Entities;

namespace GTT.Application.Repositories
{
    public interface IClassRepository : IGenericRepository<Challenge>
    {
        Task<BaseResponseModel> CreateClass(CreateClassRequestModel request);
    }
}
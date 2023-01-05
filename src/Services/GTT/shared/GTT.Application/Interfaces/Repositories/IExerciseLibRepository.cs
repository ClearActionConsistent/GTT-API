using GTT.Application.Requests.ExerciseLib;
using GTT.Application.Response;
using GTT.Domain.Entities;

namespace GTT.Application.Interfaces.Repositories
{
    public interface IExerciseLibRepository
    {
        Task<BaseResponseModel> CreateExerciseLib(CreateExerciseLibRequestModel request);
        Task<ListExerciseLibraryResponse> GetAllExerciseLib(int pageSize, int pageIndex, string filter);
    }
}

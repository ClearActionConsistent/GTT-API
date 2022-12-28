using GTT.Application.Requests.ExerciseLib;
using GTT.Application.Response;

namespace GTT.Application.Interfaces.Repositories
{
    public interface IExerciseLibRepository
    {
        Task<BaseResponseModel> CreateExerciseLib(CreateExerciseLibRequestModel request);
    }
}

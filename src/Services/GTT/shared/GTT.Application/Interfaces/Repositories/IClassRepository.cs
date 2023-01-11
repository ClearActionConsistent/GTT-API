using GTT.Application.Requests;
using GTT.Application.Response;
using GTT.Application.ViewModels;


namespace GTT.Application.Repositories
{
    public interface IClassRepository : IGenericRepository<ClassVM>
    {
        Task<BaseResponseModel> CreateClass(CreateClassRequestModel request);
        Task<ListClassResponse> GetAllClass(int pageSize, int pageIndex, string filter);
        Task<int> UpdateClass(UpdateClassData data);
    }
}
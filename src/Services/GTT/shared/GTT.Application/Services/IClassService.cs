using GTT.Application.ViewModels;

namespace GTT.Application.Services
{
    public interface IClassService
    {
        public Task<List<ClassVM>> GetClassesByNameAsync(string name);
    }
}

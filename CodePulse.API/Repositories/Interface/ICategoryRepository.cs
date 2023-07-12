using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        //only define of the method not an actual implementation
        Task<Category> CreateAsync (Category category);
    }
}

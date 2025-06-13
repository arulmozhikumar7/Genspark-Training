using ExpenseTrackerAPI.DTOs;

namespace ExpenseTrackerAPI.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryReadDto>> GetAllAsync();
        Task<CategoryReadDto?> GetByIdAsync(Guid id);
        Task<CategoryReadDto> CreateAsync(CategoryCreateDto dto);
        Task<bool> UpdateAsync(Guid id, CategoryUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}

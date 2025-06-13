using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Interfaces;
using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CategoryReadDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();
            return categories.Select(c => new CategoryReadDto
            {
                Id = c.Id,
                Name = c.Name
            });
        }

        public async Task<CategoryReadDto?> GetByIdAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) return null;

            return new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<CategoryReadDto> CreateAsync(CategoryCreateDto dto)
        {
            if (await _repository.ExistsByNameAsync(dto.Name))
                throw new Exception("Category name already exists");

            var category = new Category
            {
                Name = dto.Name
            };

            await _repository.AddAsync(category);
            await _repository.SaveChangesAsync();

            return new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<bool> UpdateAsync(Guid id, CategoryUpdateDto dto)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) return false;

            if (await _repository.ExistsByNameAsync(dto.Name, id))
                throw new Exception("Another category with the same name already exists");

            category.Name = dto.Name;
            category.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(category);
            return await _repository.SaveChangesAsync();
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) return false;

            await _repository.DeleteAsync(category);
            return await _repository.SaveChangesAsync();
        }
    }
}
